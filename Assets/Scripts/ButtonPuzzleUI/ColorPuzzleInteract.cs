using UnityEngine;

public class ColorPuzzleInteract : MonoBehaviour
{
    public GameObject promptUI;     
    public GameObject puzzlePanel;  

    public MonoBehaviour[] scriptsToDisable; 

    private bool playerInRange = false;
    private bool isOpen = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                OpenPuzzle();
            }
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    void OpenPuzzle()
    {
        isOpen = true;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePuzzle()
    {
        isOpen = false;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}