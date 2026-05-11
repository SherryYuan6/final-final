using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecurityCamera : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] string _DisplayName;
    [SerializeField] Camera LinkedCamera;
    [SerializeField] bool SyncToMainCameraConfig = true;
    [SerializeField] AudioListener CameraAudio;
    [SerializeField] Transform PivotPoint;
    [SerializeField] float DefaultPitch = 20f;
    [SerializeField] float AngleSwept = 60f;
    [SerializeField] float SweepSpeed = 6f;
    [SerializeField] int OutputTextureSize = 256;
    [SerializeField] float MaxRotationSpeed = 15f;

    [Header("Detection")]
    [SerializeField] float DetectionHalfAngle = 30f;
    [SerializeField] float DetectionRange = 20f;
    [SerializeField] float TargetVOffset = 1f;
    [SerializeField] SphereCollider DetectionTrigger;
    [SerializeField] Light DetectionLight;
    [SerializeField] Color Colour_NothingDetected = Color.green;
    [SerializeField] Color Colour_FullyDetected = Color.red;
    [SerializeField] float DetectionBuildRate = 0.5f;
    [SerializeField] float DetectionDecayRate = 0.5f;
    [SerializeField][Range(0f, 1f)] float SuspicionThreshold = 0.5f;
    [SerializeField] List<string> DetectableTags;
    [SerializeField] LayerMask DetectionLayerMask = ~0;

    [SerializeField] UnityEvent<GameObject> OnDetected = new UnityEvent<GameObject>();
    [SerializeField] UnityEvent OnAllClear = new UnityEvent();

    public RenderTexture OutputTexture { get; private set; }
    public string DisplayName => _DisplayName;
    public GameObject CurrentlyDetectedTarget { get; private set; }
    public bool HasDetectedTarget { get; private set; } = false;

    public float CurrentDetectionLevel { get; private set; } = 0f;

    float CurrentAngle = 0f;
    float CosDetectionHalfAngle;
    bool SweepClockwise = true;
    bool _isDisabled = false;

    List<SecurityConsole> CurrentlyWatchingConsoles = new List<SecurityConsole>();

    class PotentialTarget
    {
        public GameObject LinkedGO;
        public bool InFOV;
        public float DetectionLevel;
        public bool OnDetectedEventSent;
    }

    Dictionary<GameObject, PotentialTarget> AllTargets = new Dictionary<GameObject, PotentialTarget>();

    void Start()
    {
        LinkedCamera.enabled = false;
        CameraAudio.enabled = false;

        DetectionLight.color = Colour_NothingDetected;
        DetectionLight.range = DetectionRange;
        DetectionLight.spotAngle = DetectionHalfAngle * 2f;
        DetectionTrigger.radius = DetectionRange;

        CosDetectionHalfAngle = Mathf.Cos(Mathf.Deg2Rad * DetectionHalfAngle);

        if (SyncToMainCameraConfig)
        {
            LinkedCamera.clearFlags = Camera.main.clearFlags;
            LinkedCamera.backgroundColor = Camera.main.backgroundColor;
        }

        OutputTexture = new RenderTexture(OutputTextureSize, OutputTextureSize, 32);
        LinkedCamera.targetTexture = OutputTexture;
    }

    void Update()
    {
        if (_isDisabled) return;

        RefreshTargetInfo();

        Quaternion desiredRotation = PivotPoint.transform.rotation;

        if (CurrentlyDetectedTarget != null && AllTargets[CurrentlyDetectedTarget].DetectionLevel >= SuspicionThreshold)
        {
            if (AllTargets[CurrentlyDetectedTarget].InFOV)
            {
                var vecToTarget = (CurrentlyDetectedTarget.transform.position + TargetVOffset * Vector3.up -
                                   PivotPoint.transform.position).normalized;

                desiredRotation = Quaternion.LookRotation(vecToTarget, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
            }
        }
        else
        {
            CurrentAngle += SweepSpeed * Time.deltaTime * (SweepClockwise ? 1f : -1f);
            if (Mathf.Abs(CurrentAngle) >= (AngleSwept * 0.5f))
                SweepClockwise = !SweepClockwise;

            desiredRotation = PivotPoint.transform.parent.rotation * Quaternion.Euler(0f, CurrentAngle, DefaultPitch);
        }

        PivotPoint.transform.rotation = Quaternion.RotateTowards(PivotPoint.transform.rotation,
                                                                 desiredRotation,
                                                                 MaxRotationSpeed * Time.deltaTime);
    }

    void RefreshTargetInfo()
    {
        float highestDetectionLevel = 0f;
        CurrentlyDetectedTarget = null;

        foreach (var target in AllTargets)
        {
            var targetInfo = target.Value;
            bool isVisible = false;

            Vector3 vecToTarget = (targetInfo.LinkedGO.transform.position + TargetVOffset * Vector3.up -
                                   LinkedCamera.transform.position).normalized;

            if (Vector3.Dot(LinkedCamera.transform.forward, vecToTarget) >= CosDetectionHalfAngle)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(LinkedCamera.transform.position, vecToTarget,
                                    out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider.gameObject == targetInfo.LinkedGO)
                        isVisible = true;
                }
            }

            targetInfo.InFOV = isVisible;
            if (isVisible)
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionBuildRate * Time.deltaTime);

                if (targetInfo.DetectionLevel >= 1f && !targetInfo.OnDetectedEventSent)
                {
                    HasDetectedTarget = true;
                    targetInfo.OnDetectedEventSent = true;
                    OnDetected.Invoke(targetInfo.LinkedGO);
                }
            }
            else
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel - DetectionDecayRate * Time.deltaTime);

            if (targetInfo.DetectionLevel > highestDetectionLevel)
            {
                highestDetectionLevel = targetInfo.DetectionLevel;
                CurrentlyDetectedTarget = targetInfo.LinkedGO;
            }
        }

        CurrentDetectionLevel = highestDetectionLevel;

        if (CurrentlyDetectedTarget != null)
            DetectionLight.color = Color.Lerp(Colour_NothingDetected, Colour_FullyDetected, highestDetectionLevel);
        else
        {
            DetectionLight.color = Colour_NothingDetected;

            if (HasDetectedTarget)
            {
                HasDetectedTarget = false;
                OnAllClear.Invoke();
            }
        }
    }

    public void Disable()
    {
        _isDisabled = true;
        LinkedCamera.enabled = false;
        DetectionLight.enabled = false;
        DetectionTrigger.enabled = false;
        AllTargets.Clear();
        CurrentDetectionLevel = 0f;
        OnAllClear.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDisabled) return;
        if (!DetectableTags.Contains(other.tag)) return;
        AllTargets[other.gameObject] = new PotentialTarget() { LinkedGO = other.gameObject };
    }

    private void OnTriggerExit(Collider other)
    {
        if (!DetectableTags.Contains(other.tag)) return;
        AllTargets.Remove(other.gameObject);
    }

    public void StartWatching(SecurityConsole linkedConsole)
    {
        if (!CurrentlyWatchingConsoles.Contains(linkedConsole))
            CurrentlyWatchingConsoles.Add(linkedConsole);
        OnWatchersChanged();
    }

    public void StopWatching(SecurityConsole linkedConsole)
    {
        CurrentlyWatchingConsoles.Remove(linkedConsole);
        OnWatchersChanged();
    }

    void OnWatchersChanged()
    {
        LinkedCamera.enabled = CurrentlyWatchingConsoles.Count > 0;
    }
}