using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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