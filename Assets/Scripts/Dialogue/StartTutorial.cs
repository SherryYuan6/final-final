using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] introLines = new string[]
        {
            "Camera is watching...",
            "I need to destroy it first.",
            "Oh, there's a hammer nearby."
        };
        TutorialDialogueManager.instance.StartDialogue(introLines);
    }
}
