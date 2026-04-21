using UnityEngine;

public class FolderClueReveal : MonoBehaviour
{
    public GameObject folderHintPanel;
    public GameObject wrongHint;
    public GameObject trueHint;

    public void OpenClue()
    {
        if (folderHintPanel != null)
            folderHintPanel.SetActive(true);

        if (Decay.Instance != null && Decay.Instance.CanRevealTrueClue())
        {
            if (trueHint != null) trueHint.SetActive(true);
            if (wrongHint != null) wrongHint.SetActive(false);
        }
        else
        {
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