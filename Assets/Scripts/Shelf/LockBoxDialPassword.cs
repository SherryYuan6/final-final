using UnityEngine;
using TMPro;

public class LockBoxDialPassword : MonoBehaviour
{
    [Header("Password")]
    public string correctPassword = "0716";

    [Header("UI")]
    public GameObject lockBoxPanel;
    public Drawer drawer;
    public TMP_Text[] digitTexts;
    public TMP_Text messageText;

    [Header("Prompt")]
    public GameObject promptUI;

   [Header("Trigger")]
   public Collider triggerCollider;

    private int[] digits = new int[4];
    private bool playerInRange = false;
    private bool isUnlocked = false;

    void Start()
    {
        if (lockBoxPanel != null)
            lockBoxPanel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        UpdateDigitUI();

        if (messageText != null)
            messageText.text = "";

    Debug.Log("LockBox script attached on: " + gameObject.name);
    }

    void Update()
    {
        if (playerInRange && !isUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            OpenLockUI();
        }
    }

    public void OpenLockUI()
    {
        if (lockBoxPanel != null)
            lockBoxPanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (messageText != null)
            messageText.text = "";

        UpdateDigitUI();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseLockUI()
    {
        if (lockBoxPanel != null)
            lockBoxPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void IncreaseDigit(int index)
    {
        if (index < 0 || index >= digits.Length)
            return;

        digits[index]++;

        if (digits[index] > 9)
            digits[index] = 0;

        UpdateDigitUI();
        CheckPassword();
    }

    public void DecreaseDigit(int index)
    {
        if (index < 0 || index >= digits.Length)
            return;

        digits[index]--;

        if (digits[index] < 0)
            digits[index] = 9;

        UpdateDigitUI();
        CheckPassword();
    }

    void UpdateDigitUI()
    {
        for (int i = 0; i < digitTexts.Length && i < digits.Length; i++)
        {
            if (digitTexts[i] != null)
                digitTexts[i].text = digits[i].ToString();
        }
    }

    void CheckPassword()
    {
        string currentPassword = "";

        for (int i = 0; i < digits.Length; i++)
        {
            currentPassword += digits[i].ToString();
        }

        if (currentPassword == correctPassword)
        {
            UnlockBox();
        }
        else
        {
            if (messageText != null)
                messageText.text = "Wrong password.";
        }
    }

    void UnlockBox()
{
    isUnlocked = true;
    playerInRange = false;

    if (lockBoxPanel != null)
        lockBoxPanel.SetActive(false);

    if (promptUI != null)
        promptUI.SetActive(false);

    if (messageText != null)
        messageText.text = "";

    if (triggerCollider != null)
        triggerCollider.enabled = false;
    else
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    if (drawer != null)
        drawer.UnlockDrawer();
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

            if (lockBoxPanel != null && lockBoxPanel.activeSelf)
                CloseLockUI();
        }
    }
}