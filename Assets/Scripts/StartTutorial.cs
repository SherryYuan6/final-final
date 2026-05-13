using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    [Header("Player Control")]
    public MonoBehaviour playerMovement; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 禁止玩家移动
        if (playerMovement != null)
            playerMovement.enabled = false;

        string[] introLines = new string[]
        {
            "There's a camera. They are probably watching...",
            "I need to destroy it.",
            "Oh, there's a axe."
        };
         TutorialDialogueManager.instance.StartDialogue(
            introLines,
            OnDialogueFinished
        );

        void OnDialogueFinished()
    {
        // 对话结束后恢复移动
        if (playerMovement != null)
            playerMovement.enabled = true;
    }
    }
}
