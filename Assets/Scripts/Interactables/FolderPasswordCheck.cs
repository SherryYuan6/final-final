using UnityEngine;
using TMPro;

public class FolderPasswordCheck : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject folderPasswordPanel;
    public GameObject finalContentPanel; // 第二扇门密码内容界面
    public string correctPassword = "MIRA";

    public void CheckFolderPassword()
    {
        if (inputField.text == correctPassword)
        {
            TrashCanPuzzle.Instance.folderUnlocked = true;
            folderPasswordPanel.SetActive(false);
            finalContentPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Wrong folder password");
            inputField.text = "";
        }
    }
}