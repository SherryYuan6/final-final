using UnityEngine;
using TMPro;

public class Level2LockedFolderCheck : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject lockedFolderPanel;
    public GameObject doorCodePanel;
    public GameObject wrongText;

    public string correctPassword = "ARM09";

    public void CheckPassword()
    {
        if (inputField == null || lockedFolderPanel == null || doorCodePanel == null)
        {
            Debug.LogError("Level2LockedFolderCheck: missing reference!");
            return;
        }

        if (inputField.text == correctPassword)
        {
            Debug.Log("Locked folder unlocked");

            lockedFolderPanel.SetActive(false);
            doorCodePanel.SetActive(true);

            if (wrongText != null)
                wrongText.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong locked folder password");

            if (wrongText != null)
                wrongText.SetActive(true);

            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}