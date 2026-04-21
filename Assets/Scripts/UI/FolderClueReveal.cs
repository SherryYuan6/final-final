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

        Debug.Log("folderHintPanel = " + folderHintPanel);
        Debug.Log("wrongHint = " + wrongHint);
        Debug.Log("trueHint = " + trueHint);

        if (Decay.Instance == null)
        {
            Debug.LogError("Decay.Instance is NULL");
            return;
        }

        Debug.Log("currentDecay = " + Decay.Instance.currentDecay);
        Debug.Log("maxDecay = " + Decay.Instance.maxDecay);
        Debug.Log("revealThreshold = " + Decay.Instance.revealThreshold);
        Debug.Log("CanRevealTrueClue = " + Decay.Instance.CanRevealTrueClue());

        if (Decay.Instance.CanRevealTrueClue())
        {
            Debug.Log("SHOW TRUE");

            if (trueHint != null) trueHint.SetActive(true);
            if (wrongHint != null) wrongHint.SetActive(false);

            Debug.Log("trueHint activeSelf = " + trueHint.activeSelf);
            Debug.Log("wrongHint activeSelf = " + wrongHint.activeSelf);
        }
        else
        {
            Debug.Log("SHOW WRONG");

            if (wrongHint != null) wrongHint.SetActive(true);
            if (trueHint != null) trueHint.SetActive(false);

            Debug.Log("trueHint activeSelf = " + trueHint.activeSelf);
            Debug.Log("wrongHint activeSelf = " + wrongHint.activeSelf);
        }
    }

    public void CloseClue()
    {
        if (folderHintPanel != null)
            folderHintPanel.SetActive(false);
    }
}