using UnityEngine;

public class MainComputerWirePuzzleInteract : MonoBehaviour
{
    [Header("UI")]
    public GameObject wirePuzzlePanel;
    public GameObject promptUI;

    private bool playerInRange = false;

    void Update()
    {
        if (wirePuzzlePanel != null && wirePuzzlePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWirePuzzle();
            return;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenWirePuzzle();
        }
    }

    private void OpenWirePuzzle()
    {
        if (wirePuzzlePanel != null)
            wirePuzzlePanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseWirePuzzle()
    {
        if (wirePuzzlePanel != null)
            wirePuzzlePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}