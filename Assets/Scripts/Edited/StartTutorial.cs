using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] introLines = new string[]
        {
            "There's a camera. They are probably watching...",
            "I need to destroy it.",
            "Oh, there's a axe."
        };
        TutorialDialogueManager.instance.StartDialogue(introLines);
    }
}
