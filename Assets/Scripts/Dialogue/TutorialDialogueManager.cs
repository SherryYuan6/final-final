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

    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentIndex = 0;
        isDialogueActive = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
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
        isDialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}