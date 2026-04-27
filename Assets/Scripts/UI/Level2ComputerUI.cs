using UnityEngine;

public class Level2ComputerUI : MonoBehaviour
{
    public GameObject desktopPanel;
    public GameObject patternFolderPanel;
    public GameObject lockedFolderPanel;
    public GameObject doorCodePanel;

    public void CloseDesktop()
    {
        if (desktopPanel != null)
            desktopPanel.SetActive(false);

        if (patternFolderPanel != null)
            patternFolderPanel.SetActive(false);

        if (lockedFolderPanel != null)
            lockedFolderPanel.SetActive(false);

        if (doorCodePanel != null)
            doorCodePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenLockedFolderPanel()
    {
        if (lockedFolderPanel != null)
            lockedFolderPanel.SetActive(true);
    }

    public void CloseLockedFolderPanel()
    {
        if (lockedFolderPanel != null)
            lockedFolderPanel.SetActive(false);
    }

    public void ClosePatternFolderPanel()
    {
        if (patternFolderPanel != null)
            patternFolderPanel.SetActive(false);
    }

    public void CloseDoorCodePanel()
    {
        if (doorCodePanel != null)
            doorCodePanel.SetActive(false);
    }
}
