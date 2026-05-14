using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static bool InputLocked { get; private set; }

    public static bool IsUnlocked { get; private set; }

    public static void Lock()
    {
        IsUnlocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void Unlock()
    {
        IsUnlocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void SetInputLocked(bool locked)
    {
        InputLocked = locked;
    }
}