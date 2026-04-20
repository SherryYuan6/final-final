using UnityEngine;

public class FolderInteract : MonoBehaviour
{
    public GameObject folderPasswordPanel;

    public void OpenFolderPasswordPanel()
    {
        folderPasswordPanel.SetActive(true);
    }
}