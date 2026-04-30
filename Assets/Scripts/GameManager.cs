using UnityEngine;

public class TrashCanPuzzle : MonoBehaviour
{
    public static TrashCanPuzzle Instance;

    public bool trashHintViewed = false;
    public bool computerUnlocked = false;
    public bool folderUnlocked = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}