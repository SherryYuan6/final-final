using UnityEngine;

public class SubComputerChipReader : MonoBehaviour
{
    [Header("Required Item")]
    public string requiredItemID = "hard disk";
    public int requiredAmount = 1;

    [Header("UI")]
    public GameObject formulaHintPanel;
    public GameObject promptUI;          // Press E
    public GameObject needHardDriveUI;   // Need hard drive

    private bool playerInRange = false;
    private bool diskInserted = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (needHardDriveUI != null)
            needHardDriveUI.SetActive(false);

        if (formulaHintPanel != null)
            formulaHintPanel.SetActive(false);
    }

    void Update()
    {
         if (formulaHintPanel != null && formulaHintPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
    {
        CloseFormulaHint();
        return;
    }
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryReadDisk();
        }
    }

    private void TryReadDisk()
    {
        if (!diskInserted)
        {
            if (ToolBarUI.Instance == null)
            {
                Debug.LogWarning("ToolBarUI.Instance is null.");
                return;
            }

            int count = ToolBarUI.Instance.CountItem(requiredItemID);

            Debug.Log("Required item ID = " + requiredItemID);
            Debug.Log("Item count = " + count);

            if (count < requiredAmount)
            {
                Debug.Log("Need hard disk.");

                if (needHardDriveUI != null)
                    needHardDriveUI.SetActive(true);

                return;
            }

            ToolBarUI.Instance.RemoveItems(requiredItemID, requiredAmount);
            diskInserted = true;

            Debug.Log("Hard disk inserted into auxiliary terminal.");

            if (needHardDriveUI != null)
                needHardDriveUI.SetActive(false);
        }

        OpenFormulaHint();
    }

    private void OpenFormulaHint()
    {
        if (formulaHintPanel != null)
            formulaHintPanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (needHardDriveUI != null)
            needHardDriveUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseFormulaHint()
    {
        if (formulaHintPanel != null)
            formulaHintPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptUI != null && !diskInserted)
                promptUI.SetActive(true);

            if (needHardDriveUI != null)
                needHardDriveUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);

            if (needHardDriveUI != null)
                needHardDriveUI.SetActive(false);
        }
    }
}