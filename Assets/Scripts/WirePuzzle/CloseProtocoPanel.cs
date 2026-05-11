using UnityEngine;

public class EscClosePanel : MonoBehaviour
{
    public GameObject panelToClose;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelToClose != null)
                panelToClose.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}