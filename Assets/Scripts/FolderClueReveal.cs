using UnityEngine;
using System.Collections;

public class FolderClueReveal : MonoBehaviour
{
    public GameObject folderHintPanel;
    public GameObject wrongHint;
    public GameObject trueHint;

    private bool hasTriggeredThought = false;
    private bool lastWasWrong = false;

    public void OpenClue()
    {
        folderHintPanel.SetActive(true);
        UpdateClueDisplay();
    }

    void Update()
    {
        if (folderHintPanel == null || !folderHintPanel.activeSelf) return;

        UpdateClueDisplay();
        if (!hasTriggeredThought && lastWasWrong)
        {
            StartCoroutine(DelayedThought());
            hasTriggeredThought = true;
        }
    }

    IEnumerator DelayedThought()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        if (folderHintPanel.activeSelf && wrongHint.activeSelf)
        {
            TutorialDialogueManager.instance.StartDialogue(new string[]
            {
                "That doesn't seem right...",
                "2333333333? It feels like a placeholder.",
                "I feel like I should wait a bit or press K for the true hint.",
                "Later to restore sanity, find pills to consume."
            });
        }
    }

    void UpdateClueDisplay()
    {
        bool showTrue = ChipManager.Instance != null && ChipManager.Instance.CanRevealTrueClue();

        if (trueHint != null) trueHint.SetActive(showTrue);
        if (wrongHint != null) wrongHint.SetActive(!showTrue);

        lastWasWrong = !showTrue;
    }
}