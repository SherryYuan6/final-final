using UnityEngine;

public class PanelCursorUnlock : MonoBehaviour
{
    void OnEnable()
    {
        UnlockCursor();
    }

    void Update()
    {
        // 只要这个 panel 开着，就持续保证鼠标可见
        UnlockCursor();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
