using UnityEngine;

public class PatternClueReveal : MonoBehaviour
{
    public GameObject patternFolderPanel;
    public GameObject wrongPattern;
    public GameObject rightPattern;

    private bool lastRevealState;

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
        if (patternFolderPanel == null || !patternFolderPanel.activeSelf)
            return;
        if(ChipManager.Instance == null) return;
        
        bool currentState = ChipManager.Instance.CanRevealTrueClue();
        if (currentState != lastRevealState)
        {
            UpdatePatternDisplay();
        }
    }

    void UpdatePatternDisplay()
    {
        if (ChipManager.Instance == null) return;

        bool showRight = ChipManager.Instance.CanRevealTrueClue();
        lastRevealState = showRight;

        if (rightPattern != null)
            rightPattern.SetActive(showRight);

        if (wrongPattern != null)
            wrongPattern.SetActive(!showRight);

        //Debug.Log("Pattern updated. Show right = " + showRight);
    }
}