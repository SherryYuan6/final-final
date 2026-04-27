using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadScene : MonoBehaviour
{
    public string nextScene;
    public float delay = 5f;
    void Start()
    {
        Invoke(nameof(LoadNext), delay);
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             LoadNext();
        }
    }


    void LoadNext()
    {
        SceneManager.LoadScene("Labatotry");
    }
}