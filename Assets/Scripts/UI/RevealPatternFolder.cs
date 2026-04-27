using UnityEngine;

public class PatternClueReveal : MonoBehaviour
{
    public GameObject patternFolderPanel;
    public GameObject wrongPattern;
    public GameObject rightPattern;

    private bool lastRevealState = false;

    public void OpenPattern()
    {
        if (patternFolderPanel != null)
            patternFolderPanel.SetActive(true);

        UpdatePatternDisplay();
    }

    public void ClosePattern()
    {
        if (patternFolderPanel != null)
            patternFolderPanel.SetActive(false);
    }

    void Update()
    {
        if (patternFolderPanel != null && patternFolderPanel.activeSelf && Decay.Instance != null)
        {
            bool currentState = Decay.Instance.CanRevealTrueClue();

            if (currentState != lastRevealState)
            {
                UpdatePatternDisplay();
            }
        }
    }

    void UpdatePatternDisplay()
    {
        if (Decay.Instance == null) return;

        bool showRight = Decay.Instance.CanRevealTrueClue();
        lastRevealState = showRight;

        if (rightPattern != null)
            rightPattern.SetActive(showRight);

        if (wrongPattern != null)
            wrongPattern.SetActive(!showRight);

        Debug.Log("Pattern updated. Show right = " + showRight);
    }
}