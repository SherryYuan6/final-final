using UnityEngine;

public class FolderClueReveal : MonoBehaviour
{
    public GameObject folderHintPanel;
    public GameObject wrongHint;
    public GameObject trueHint;

    private bool lastRevealState = false;

    public void OpenClue()
    {
        if (folderHintPanel != null)
            folderHintPanel.SetActive(true);

        UpdateClueDisplay();
    }

    public void CloseClue()
    {
        if (folderHintPanel != null)
            folderHintPanel.SetActive(false);
    }

    void Update()
    {
        if (folderHintPanel != null && folderHintPanel.activeSelf && Decay.Instance != null)
        {
            bool currentState = Decay.Instance.CanRevealTrueClue();

            if (currentState != lastRevealState)
            {
                UpdateClueDisplay();
            }
        }
    }

    void UpdateClueDisplay()
    {
        if (Decay.Instance == null) return;

        bool showTrue = Decay.Instance.CanRevealTrueClue();
        lastRevealState = showTrue;

        if (trueHint != null) trueHint.SetActive(showTrue);
        if (wrongHint != null) wrongHint.SetActive(!showTrue);
    }
}