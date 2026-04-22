using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TutorialDialogueManager : MonoBehaviour
{
    public static TutorialDialogueManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Settings")]
    public KeyCode nextKey = KeyCode.Space;

    private string[] lines;
    private int currentLine;
    private bool isShowing = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isShowing && Input.GetKeyUp(nextKey))
        {
            ShowNextLine();
        }
    }
    
    public void StartDialogue(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;
        
        lines = newLines;
        currentLine = 0;
        isShowing = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = lines[currentLine];

        FindAnyObjectByType<FirstPersonMovement>().enabled = false;
    }

    public void ShowNextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = lines[currentLine];
        }
    }
        public void EndDialogue()
        {
            isShowing = false;
            dialoguePanel.SetActive(false);

            FindAnyObjectByType<FirstPersonMovement>().enabled = true;
        }

        public bool Isshowing()
        {
            return isShowing;
        }
}
