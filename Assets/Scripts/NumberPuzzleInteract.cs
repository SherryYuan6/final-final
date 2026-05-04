using UnityEngine;

public class NumberPuzzleInteract : MonoBehaviour
{
    public GameObject promptUI;
    public GameObject puzzlePanel;
    public GameObject rewardPanel;

    public NumberPuzzleManager puzzleManager;

    public MonoBehaviour[] scriptsToDisable;

    private bool playerInRange = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (puzzleManager != null && puzzleManager.IsSolved())
            {
                OpenRewardPanel();
            }
            else
            {
                OpenPuzzlePanel();
            }
        }
    }

    void OpenPuzzlePanel()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OpenRewardPanel()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseAllPanels()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

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

            CloseAllPanels();
        }
    }
}