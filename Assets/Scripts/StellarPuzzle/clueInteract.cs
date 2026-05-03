using UnityEngine;

public class clueInteract: MonoBehaviour
{
    public GameObject promptUI;     
    public GameObject cluePanel;  

    public MonoBehaviour[] scriptsToDisable; 

    private bool playerInRange = false;
    private bool isOpen = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (cluePanel != null)
            cluePanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                OpenClue();
            }
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseClue();
        }
    }

    void OpenClue()
    {
        isOpen = true;

        if (cluePanel != null)
            cluePanel.SetActive(true);

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

    public void CloseClue()
    {
        isOpen = false;

        if (cluePanel != null)
            cluePanel.SetActive(false);

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