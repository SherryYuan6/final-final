using UnityEngine;

public class Drawer : MonoBehaviour
{
    public GameObject promptUI;

    [Header("Drawer Models")]
    public GameObject closeDrawer;
    public GameObject openDrawer;

    [Header("Lock UI")]
    public GameObject drawerLockPanel;
    public GameObject patternLockPanel;

    [Header("Small Box")]
    public GameObject chestVariant;

    [Header("Disable while UI is open")]
    public MonoBehaviour[] scriptsToDisable;

    public AudioSource sound;

    private bool playerInRange = false;
    private bool isUnlocked = false;

    void Start()
    {
        if (closeDrawer != null)
            closeDrawer.SetActive(true);

        if (openDrawer != null)
            openDrawer.SetActive(false);

        if (chestVariant != null)
            chestVariant.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(false);

        if (patternLockPanel != null)
            patternLockPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            OpenDrawerLockUI();
        }
    }

    void OpenDrawerLockUI()
    {
        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        SetPlayerControl(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockDrawer()
    {
        isUnlocked = true;
        playerInRange = false;

        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (closeDrawer != null)
            closeDrawer.SetActive(false);

        if (openDrawer != null)
            openDrawer.SetActive(true);

        if (chestVariant != null)
            chestVariant.SetActive(true);

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        if (sound != null)
            sound.Play();

        if (patternLockPanel != null)
        {
            patternLockPanel.SetActive(true);

            SetPlayerControl(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }

        FinishPatternLock();
    }

    public void FinishPatternLock()
    {
        if (patternLockPanel != null)
            patternLockPanel.SetActive(false);

        SetPlayerControl(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void SetPlayerControl(bool enabled)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = enabled;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUnlocked)
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

            if (drawerLockPanel != null && drawerLockPanel.activeSelf)
                drawerLockPanel.SetActive(false);
        }
    }
}