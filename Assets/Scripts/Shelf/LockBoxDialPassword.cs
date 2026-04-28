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
    //public GameObject cluePanel;

    [Header("Prompt")]
    public GameObject promptUI;

    private int[] digits = new int[4];
    private bool playerInRange = false;
    private bool isUnlocked = false;

    void Start()
    {
        lockBoxPanel.SetActive(false);

        //if (cluePanel != null)
            //cluePanel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        UpdateDigitUI();
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
        lockBoxPanel.SetActive(true);
        messageText.text = "";
        UpdateDigitUI();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseLockUI()
    {
        lockBoxPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void IncreaseDigit(int index)
    {
        digits[index]++;

        if (digits[index] > 9)
            digits[index] = 0;

        UpdateDigitUI();
        CheckPassword();
    }

    public void DecreaseDigit(int index)
    {
        digits[index]--;

        if (digits[index] < 0)
            digits[index] = 9;

        UpdateDigitUI();
        CheckPassword();
    }

    void UpdateDigitUI()
    {
        for (int i = 0; i < digitTexts.Length; i++)
        {
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
            messageText.text = "Wrong password.";
        }
    }

    void UnlockBox()
    {
        isUnlocked = true;

        lockBoxPanel.SetActive(false);

        //if (cluePanel != null)
            //cluePanel.SetActive(true);

        if (promptUI != null)
            promptUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        drawer.UnlockDrawer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isUnlocked)
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);

            if (lockBoxPanel.activeSelf)
                CloseLockUI();
        }
    }
}