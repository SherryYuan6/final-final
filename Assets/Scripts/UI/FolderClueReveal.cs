using UnityEngine;

public class FolderClueReveal : MonoBehaviour
{
    public GameObject folderHintPanel;
    public GameObject wrongHint;
    public GameObject trueHint;

    public void OpenClue()
    {
        Debug.Log("OpenClue called");

        if (folderHintPanel != null)
            folderHintPanel.SetActive(true);

        if (Decay.Instance == null)
        {
            Debug.LogError("Decay.Instance is null");
            return;
        }

        Debug.Log("Current Decay = " + Decay.Instance.currentDecay);
        Debug.Log("Reveal Threshold = " + Decay.Instance.revealThreshold);

        if (Decay.Instance.CanRevealTrueClue())
        {
            Debug.Log("Show TRUE clue");

            if (trueHint != null) trueHint.SetActive(true);
            if (wrongHint != null) wrongHint.SetActive(false);
        }
        else
        {
            Debug.Log("Show WRONG clue");

            if (wrongHint != null) wrongHint.SetActive(true);
            if (trueHint != null) trueHint.SetActive(false);
        }
    }

    public void CloseClue()
    {
        if (folderHintPanel != null)
            folderHintPanel.SetActive(false);
    }
}