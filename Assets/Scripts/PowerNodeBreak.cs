using UnityEngine;

public class PowerNodeBreak : MonoBehaviour
{
    public static int brokenPowerCount = 0;

    [Header("Power Node Settings")]
    public int totalPowerNeeded = 2;
    public string requiredItemID = "hammer";

    [Header("Models")]
    public GameObject normalModel;
    public GameObject brokenModel;
    public GameObject partToDisappear;

    [Header("UI")]
    public GameObject promptUI;
    public GameObject needHammerUI;

    [Header("Audio")]
    public AudioSource breakSound;

    private bool playerInRange = false;
    private bool isBroken = false;

    void Start()
    {
        if (normalModel != null)
            normalModel.SetActive(true);

        if (brokenModel != null)
            brokenModel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (needHammerUI != null)
            needHammerUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isBroken && Input.GetKeyDown(KeyCode.E))
        {
            TryBreakPower();
        }
    }

    private void TryBreakPower()
    {
        if (ToolBarUI.Instance == null)
        {
            Debug.LogWarning("ToolBarUI.Instance is null.");
            return;
        }

        int count = ToolBarUI.Instance.CountItem(requiredItemID);

        Debug.Log("Checking tool: " + requiredItemID + ", count = " + count);

        if (count <= 0)
        {
            if (needHammerUI != null)
                needHammerUI.SetActive(true);

            Debug.Log("Need hammer to break power node.");
            return;
        }

        BreakPowerNode();
    }

    private void BreakPowerNode()
    {
        isBroken = true;
        brokenPowerCount++;

        if (normalModel != null)
            normalModel.SetActive(false);

        if (brokenModel != null)
            brokenModel.SetActive(true);

        if (partToDisappear != null)
            partToDisappear.SetActive(false);

        if (breakSound != null)
            breakSound.Play();

        if (promptUI != null)
            promptUI.SetActive(false);

        if (needHammerUI != null)
            needHammerUI.SetActive(false);

        Debug.Log("Power node broken. Count = " + brokenPowerCount);
    }

    public static bool AllPowerBroken(int requiredCount)
    {
        return brokenPowerCount >= requiredCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBroken)
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);

            if (needHammerUI != null)
                needHammerUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);

            if (needHammerUI != null)
                needHammerUI.SetActive(false);
        }
    }
}