using UnityEngine;
using TMPro;

public class PasswordUI : MonoBehaviour
{
    public static PasswordUI Instance;

    public GameObject passwordPanel;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;

    private PasswordDoor currentDoor;
    private bool isOpen;

    public bool IsOpen => isOpen;

    void Awake()
    {
        Instance = this;
        passwordPanel.SetActive(false);
    }

    public void OpenUI(PasswordDoor door)
    {
        currentDoor = door;
        isOpen = true;

        passwordPanel.SetActive(true);
        passwordInput.text = "";
        resultText.text = "";

        CursorManager.SetInputLocked(true);
        CursorManager.Unlock();
        Time.timeScale = 0f;
    }

    public void CloseUI()
    {
        isOpen = false;
        currentDoor = null;

        passwordPanel.SetActive(false);

        CursorManager.SetInputLocked(false);
        CursorManager.Lock();
        Time.timeScale = 1f;
    }

    public void SubmitPassword()
    {
        if (currentDoor == null) return;

        if (currentDoor.CheckPassword(passwordInput.text))
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
            SubmitPassword();

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseUI();
    }
}