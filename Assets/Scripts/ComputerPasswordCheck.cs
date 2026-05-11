using UnityEngine;
using TMPro;

public class ComputerPasswordCheck : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject passwordPanel;
    public GameObject desktopPanel;
    public string correctPassword = "3046";

    public void CheckPassword()
    {
        if (inputField == null || passwordPanel == null || desktopPanel == null)
        {
            //Debug.LogError("ComputerPasswordCheck: missing reference!");
            return;
        }

        if (inputField.text == correctPassword)
        {
            if (ChipManager.Instance != null) ChipManager.Instance.LoseIntegrity(6f);
            passwordPanel.SetActive(false);
            desktopPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            //Debug.Log("Wrong password");
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    public void CloseComputer()
    {
        if (desktopPanel != null)
            desktopPanel.SetActive(false);

        if (passwordPanel != null)
            passwordPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}