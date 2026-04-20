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
        if (inputField.text == correctPassword)
        {
            GameManager.Instance.computerUnlocked = true;
            passwordPanel.SetActive(false);
            desktopPanel.SetActive(true);
            Time.timeScale = 0f; // 仍停在桌面界面
        }
        else
        {
            Debug.Log("Wrong password");
            inputField.text = "";
        }
    }
}
