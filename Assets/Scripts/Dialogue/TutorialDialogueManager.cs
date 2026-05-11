using TMPro;
using UnityEngine;

public class TutorialDialogueManager : MonoBehaviour
{
    public static TutorialDialogueManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private string[] currentLines;
    private int currentIndex;
    private bool isDialogueActive = false;
    private System.Action onDialogueFinished;

    private void Awake()
    {
        instance = this;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(string[] lines, System.Action finishedCallback = null)
    {
        currentLines = lines;
        currentIndex = 0;

        onDialogueFinished = finishedCallback;

        isDialogueActive = true;

        dialoguePanel.SetActive(true);

        dialogueText.text = currentLines[currentIndex];
    }

    void ShowNextLine()
    {
        currentIndex++;

        if (currentIndex < currentLines.Length)
        {
            dialogueText.text = currentLines[currentIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
    dialoguePanel.SetActive(false);

    if (onDialogueFinished != null)
    {
        onDialogueFinished.Invoke();
        onDialogueFinished = null;
    }
    }
}