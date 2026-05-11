using UnityEngine;

public class ThirdLevelStart : MonoBehaviour
{
    [Header("Player Control")]
    public MonoBehaviour playerMovement; 

    void Start()
    {
        // 禁止玩家移动
        if (playerMovement != null)
            playerMovement.enabled = false;

        string[] introLines = new string[]
        {
            "The exit door was locked.",
            "I need to find the power source that control it.",
        };

        TutorialDialogueManager.instance.StartDialogue(
            introLines,
            OnDialogueFinished
        );
    }

    void OnDialogueFinished()
    {
        // 对话结束后恢复移动
        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}