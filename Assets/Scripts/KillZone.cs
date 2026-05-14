using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    public string restartScene = "EndScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TutorialDialogueManager.instance.StartDialogue(
            new string[]
            {
                "What.",
                "What is happening.",
                "Oh shit"
            },
            () =>
            {
                SceneManager.LoadScene(restartScene);
            }
        );
    }
}