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
            Debug.LogError("ComputerPasswordCheck: missing reference!");
            return;
        }

        if (inputField.text == correctPassword)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.computerUnlocked = true;
            }

            passwordPanel.SetActive(false);
            desktopPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("Wrong password");
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}