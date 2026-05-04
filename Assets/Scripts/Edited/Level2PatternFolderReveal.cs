using UnityEngine;

public class Level2PatternFolderReveal : MonoBehaviour
{
    public GameObject patternFolderPanel;
    public GameObject wrongPatternHint;
    public GameObject truePatternHint;

    public void OpenPatternFolder()
    {
        //Debug.Log("OpenPatternFolder called");

        if (patternFolderPanel != null)
            patternFolderPanel.SetActive(true);

        bool show = ChipManager.Instance == null && ChipManager.Instance.CanRevealTrueClue();
        //{
        //    Debug.LogWarning("Decay.Instance is null");
        //    ShowWrongHint();
        //    return;
        //}

        //Debug.Log("currentDecay = " + ChipManager.Instance.currentDecay);
        //Debug.Log("revealThreshold = " + ChipManager.Instance.revealThreshold);
        //Debug.Log("CanRevealTrueClue = " + ChipManager.Instance.CanRevealTrueClue());
        if (truePatternHint != null) truePatternHint.SetActive(show);
        if (wrongPatternHint != null) wrongPatternHint.SetActive(!show);
    }
}
    //    if (ChipManager.Instance.CanRevealTrueClue())
    //    {
    //        ShowTrueHint();
    //    }
    //    else
    //    {
    //        ShowWrongHint();
    //    }
    //}

    //private void ShowTrueHint()
    //{
    //    Debug.Log("SHOW TRUE PATTERN");

    //    if (truePatternHint != null)
    //        truePatternHint.SetActive(true);

    //    if (wrongPatternHint != null)
    //        wrongPatternHint.SetActive(false);
    //}

    //private void ShowWrongHint()
    //{
    //    Debug.Log("SHOW WRONG PATTERN");

    //    if (wrongPatternHint != null)
    //        wrongPatternHint.SetActive(true);

    //    if (truePatternHint != null)
    //        truePatternHint.SetActive(false);
