using UnityEngine;

public class NewDialogue : MonoBehaviour
{
    [Header("Player Control")]
    public MonoBehaviour playerMovement;
    void Start()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        string[] introLines = new string[]
        {
            "I need to get a way to enter the labatory",
            "I should look around."
        };

        TutorialDialogueManager.instance.StartDialogue(
           introLines,
           OnDialogueFinished
       );

        void OnDialogueFinished()
        {
            if (playerMovement != null)
                playerMovement.enabled = true;
        }
    }
}
