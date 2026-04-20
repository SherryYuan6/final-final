using UnityEngine;

public class FolderUI : MonoBehaviour
{
    public GameObject desktopPanel;
    public GameObject folderHintPanel;

    public void OpenFolderHint()
    {
        folderHintPanel.SetActive(true);
    }

    public void CloseFolderHint()
    {
        folderHintPanel.SetActive(false);
    }

    public void CloseDesktop()
    {
        desktopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}