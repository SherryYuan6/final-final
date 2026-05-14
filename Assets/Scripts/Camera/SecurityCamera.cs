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
    [SerializeField] Transform CameraFacing;
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
    [SerializeField] float DetectionBuildRate = 3.5f;
    [SerializeField] float DetectionDecayRate = 0.5f;
    [SerializeField][Range(0f, 1f)] float SuspicionThreshold = 0.5f;
    [SerializeField] List<string> DetectableTags;
    [SerializeField] LayerMask DetectionLayerMask = ~0;

    [SerializeField] UnityEvent<GameObject> OnDetected = new UnityEvent<GameObject>();
    [SerializeField] UnityEvent OnAllClear = new UnityEvent();

    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Transform EnemySpawnPoint;
    bool _spawnTimerRunning = false;
    float _spawnTimer = 0f;

    public RenderTexture OutputTexture { get; private set; }
    public string DisplayName => _DisplayName;
    public GameObject CurrentlyDetectedTarget { get; private set; }
    public bool HasDetectedTarget { get; private set; } = false;
    public bool IsLockedOnTarget { get; private set; } = false;
    public float CurrentDetectionLevel { get; private set; } = 0f;

    GameObject _lockedTarget;
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

        if (IsLockedOnTarget && _lockedTarget != null)
        {
            Vector3 targetPos = _lockedTarget.transform.position + TargetVOffset * Vector3.up;

            Vector3 dirToTarget = (targetPos - PivotPoint.transform.position).normalized;

            Debug.Log("PivotRight:" + PivotPoint.transform.right + " PivotForward:" + PivotPoint.transform.forward + " dirToTarget:" + dirToTarget);
        }
        else
        {
            CurrentAngle += SweepSpeed * Time.deltaTime * (SweepClockwise ? 1f : -1f);
            if (Mathf.Abs(CurrentAngle) >= (AngleSwept * 0.5f))
                SweepClockwise = !SweepClockwise;

            Quaternion desiredRotation = PivotPoint.transform.parent.rotation * Quaternion.Euler(0f, CurrentAngle, DefaultPitch);
            PivotPoint.transform.rotation = Quaternion.RotateTowards(PivotPoint.transform.rotation,
                                                                     desiredRotation,
                                                                     MaxRotationSpeed * Time.deltaTime);
        }

        if (_spawnTimerRunning)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= 2f)
            {
                _spawnTimerRunning = false;
                if (EnemyPrefab != null && EnemySpawnPoint != null)
                {
                    Instantiate(EnemyPrefab, EnemySpawnPoint.position, EnemySpawnPoint.rotation);
                    TutorialDialogueManager.instance.StartDialogue(new string[]
                    {
        "I stayed under the alarm for too long.",
        "There are going to be guards soon.",
        "I need to move. Now."
                    });
                }
            }
        }
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
                                   CameraFacing.transform.position).normalized;

            if (Vector3.Dot(CameraFacing.transform.forward, vecToTarget) >= CosDetectionHalfAngle)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(CameraFacing.transform.position, vecToTarget,
                    out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider.transform.IsChildOf(targetInfo.LinkedGO.transform) ||
                        hitInfo.collider.gameObject == targetInfo.LinkedGO)
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
                    IsLockedOnTarget = true;
                    _spawnTimerRunning = true;
                    _spawnTimer = 0f;
                    _lockedTarget = targetInfo.LinkedGO;
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
        _spawnTimerRunning = false;
        _spawnTimer = 0f;
        _isDisabled = true;
        LinkedCamera.enabled = false;
        DetectionLight.enabled = false;
        DetectionTrigger.enabled = false;
        IsLockedOnTarget = false;
        _lockedTarget = null;
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