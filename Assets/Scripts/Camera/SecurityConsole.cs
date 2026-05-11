using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecurityConsole : MonoBehaviour
{
    [SerializeField] List<SecurityCamera> LinkedCameras;
    [SerializeField] RawImage CameraImage;
    [SerializeField] TextMeshProUGUI ActiveCameraLabel;
    [SerializeField] bool AutoswitchEnable = false;
    [SerializeField] float AutoswitchStartTime = 10f;
    [SerializeField] float AutoswitchInterval = 15f;

    [Header("Detection Penalty")]
    [SerializeField] float IntegrityPenalty = 20f;

    float TimeUntilNextAutoswitch = -1f;

    public int ActiveCameraIndex { get; private set; } = -1;
    public SecurityCamera ActiveCamera => ActiveCameraIndex < 0 ? null : LinkedCameras[ActiveCameraIndex];

    void Start()
    {
        ActiveCameraLabel.text = "Camera: None";
    }

    void Update()
    {
        if (AutoswitchEnable)
        {
            TimeUntilNextAutoswitch -= Time.deltaTime;
            if (TimeUntilNextAutoswitch < 0)
            {
                TimeUntilNextAutoswitch = AutoswitchInterval;
                SelectNextCamera();
            }
        }
    }

    public void OnClicked()
    {
        SelectNextCamera();
        if (AutoswitchEnable)
            TimeUntilNextAutoswitch = AutoswitchStartTime;
    }

    void SelectNextCamera()
    {
        var previousCamera = ActiveCamera;
        ActiveCameraIndex = (ActiveCameraIndex + 1) % LinkedCameras.Count;

        if (previousCamera != null)
            previousCamera.StopWatching(this);

        ActiveCamera.StartWatching(this);
        ActiveCameraLabel.text = $"Camera: {ActiveCamera.DisplayName}";
        CameraImage.texture = ActiveCamera.OutputTexture;
    }

    public void OnDetected(GameObject target)
    {
        CameraDetection.Instance?.StartDetection();
        ChipManager.Instance?.LoseIntegrity(IntegrityPenalty);

        TutorialDialogueManager.instance?.StartDialogue(new string[]
        {
            "Something noticed me.",
            "I need to move."
        });
    }

    public void OnAllClear()
    {
        CameraDetection.Instance?.EndDetection();
    }
}