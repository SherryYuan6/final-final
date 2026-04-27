using UnityEngine;

public class SimplePanClose : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        if (panelToClose != null)
            panelToClose.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  }
}