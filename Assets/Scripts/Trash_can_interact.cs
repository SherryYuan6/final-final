using UnityEngine;

public class TrashCanInteract : MonoBehaviour
{
    public GameObject hintPanel;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hintPanel.SetActive(true);
            GameManager.Instance.trashHintViewed = true;
            Time.timeScale = 0f; // 暂停游戏，可选
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}