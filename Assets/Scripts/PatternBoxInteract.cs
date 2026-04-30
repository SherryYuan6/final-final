using UnityEngine;

public class PatternBoxInteract : MonoBehaviour
{
    public GameObject patternLockPanel;
    public GameObject interactHint;

    [Header("Disable while UI is open")]
    public MonoBehaviour[] scriptsToDisable;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenPatternLock();
        }
    }

    public void OpenPatternLock()
    {
        if (patternLockPanel != null)
            patternLockPanel.SetActive(true);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Pattern lock opened. Cursor unlocked.");
    }

    public void ClosePatternLock()
    {
        if (patternLockPanel != null)
            patternLockPanel.SetActive(false);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Pattern lock closed. Cursor locked.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactHint != null)
                interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }
}