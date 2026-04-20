using UnityEngine;

public class HintPanelUI : MonoBehaviour
{
    public void ClosePanel()
    {
        gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}
