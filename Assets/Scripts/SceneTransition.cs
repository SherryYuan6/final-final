using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public string firstSceneName = "Tutorial";
    //void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //    ChipManager chip = GetComponent<ChipManager>();
    //    if (chip != null) DontDestroyOnLoad(chip.gameObject);
    //}
    public void StartGame()
    {
        SceneManager.LoadScene(firstSceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}