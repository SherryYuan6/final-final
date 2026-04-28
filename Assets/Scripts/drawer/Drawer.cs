using UnityEngine;

public class Drawer : MonoBehaviour
{
    public GameObject promptUI;

    [Header("Drawer Models")]
    public GameObject closeDrawer;
    public GameObject openDrawer; 
    
    [Header("First Lock UI")]
    public GameObject drawerLockPanel;

    [Header("Small Box")]
    public GameObject chestVariant;

    [Header("Disable while UI is open")]
    public MonoBehaviour[] scriptsToDisable;

    public AudioSource sound;

    private bool playerInRange = false;
    private bool isUnlocked = false;

    void Start()
    {
        closeDrawer.SetActive(true);
        openDrawer.SetActive(false);

        if (chestVariant != null)
            chestVariant.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isUnlocked)
            {
                OpenDrawerLockUI();
            }
        }
    }

    void OpenDrawerLockUI()
    {
        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(true);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockDrawer()
    {
        isUnlocked = true;

        if (drawerLockPanel != null)
            drawerLockPanel.SetActive(false);

        closeDrawer.SetActive(false);
        openDrawer.SetActive(true);

        if (chestVariant != null)
            chestVariant.SetActive(true);

        if (sound != null)
            sound.Play();

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

            if (!isUnlocked && promptUI != null)
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