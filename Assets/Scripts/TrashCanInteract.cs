using UnityEngine;

public class TrashHintInteract : MonoBehaviour
{
    public GameObject hintPanel;

    private bool playerInRange;
    private bool isOpen;

    void Update()
    {
        if (isOpen)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
            return;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenPanel();
        }
    }

    void OpenPanel()
    {
        isOpen = true;

        hintPanel.SetActive(true);

        CursorManager.Unlock();   
        Time.timeScale = 0f;
    }

    void ClosePanel()
    {
        isOpen = false;

        hintPanel.SetActive(false);

        CursorManager.Lock();  
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isOpen)
                ClosePanel();
        }
    }
}