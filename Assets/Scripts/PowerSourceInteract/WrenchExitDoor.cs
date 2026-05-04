using UnityEngine;
using UnityEngine.SceneManagement;

public class WrenchExitDoor : MonoBehaviour
{
    [Header("Requirements")]
    public int requiredBrokenPowerCount = 2;
    public string requiredWrenchID = "wrench";

    [Header("Door Models")]
    public GameObject closedDoor;
    public GameObject openDoor;
    public GameObject screwModel;

    [Header("UI")]
    public GameObject promptUI;
    public GameObject needPowerOffUI;
    public GameObject needWrenchUI;

    [Header("Scene Transition")]
    public bool loadNextScene = true;
    public string nextSceneName = "Level4";

    private bool playerInRange = false;
    private bool isOpened = false;

    void Start()
    {
        if (closedDoor != null)
            closedDoor.SetActive(true);

        if (openDoor != null)
            openDoor.SetActive(false);

        HideAllUI();
    }

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenExit();
        }
    }

    private void TryOpenExit()
    {
        HideAllUI();

        if (!PowerNodeBreak.AllPowerBroken(requiredBrokenPowerCount))
        {
            if (needPowerOffUI != null)
                needPowerOffUI.SetActive(true);

            Debug.Log("Power is still active. Break power nodes first.");
            return;
        }

        if (ToolBarUI.Instance == null)
        {
            Debug.LogWarning("ToolBarUI.Instance is null.");
            return;
        }

        int wrenchCount = ToolBarUI.Instance.CountItem(requiredWrenchID);

        Debug.Log("Checking wrench: " + requiredWrenchID + ", count = " + wrenchCount);

        if (wrenchCount <= 0)
        {
            if (needWrenchUI != null)
                needWrenchUI.SetActive(true);

            Debug.Log("Need wrench to remove screws.");
            return;
        }

        OpenExitDoor();
    }

    private void OpenExitDoor()
    {
        isOpened = true;

        if (closedDoor != null)
            closedDoor.SetActive(false);

        if (openDoor != null)
            openDoor.SetActive(true);

        if (screwModel != null)
            screwModel.SetActive(false);

        HideAllUI();

        Debug.Log("Exit door opened with wrench.");

        if (loadNextScene)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void HideAllUI()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (needPowerOffUI != null)
            needPowerOffUI.SetActive(false);

        if (needWrenchUI != null)
            needWrenchUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerInRange = true;

            HideAllUI();

            if (!PowerNodeBreak.AllPowerBroken(requiredBrokenPowerCount))
            {
                if (needPowerOffUI != null)
                    needPowerOffUI.SetActive(true);
            }
            else
            {
                if (promptUI != null)
                    promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideAllUI();
        }
    }
}