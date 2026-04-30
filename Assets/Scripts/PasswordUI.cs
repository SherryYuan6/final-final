using UnityEngine;
using TMPro;

public class PasswordUI : MonoBehaviour
{
    public static PasswordUI Instance;

    [Header("UI Objects")]
    public GameObject passwordPanel;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;

    private PasswordDoor currentDoor;
    private bool isOpen = false;

    void Awake()
    {
        Instance = this;

        if (passwordPanel != null)
            passwordPanel.SetActive(false);
    }

    public void OpenUI(PasswordDoor door)
    {
        currentDoor = door;
        isOpen = true;

        passwordPanel.SetActive(true);
        passwordInput.text = "";
        resultText.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; 
    }

    public void CloseUI()
    {
        isOpen = false;
        currentDoor = null;

        passwordPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }

    public void SubmitPassword()
    {
        if (currentDoor == null) return;

        string input = passwordInput.text;

        if (currentDoor.CheckPassword(input))
        {
            resultText.text = "Correct!";
            currentDoor.OpenDoor();
            CloseUI();
        }
        else
        {
            resultText.text = "Wrong Password";
        }
    }

    void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitPassword();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }
}