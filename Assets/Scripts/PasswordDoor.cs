using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasswordDoor : MonoBehaviour
{
    [Header("Password")]
    public string correctPassword = "";

    [Header("Door Models")]
    public GameObject DoorClose2;
    public GameObject DoorOpen2;

    [Header("Scene")]
    public string nextSceneName = "";
    public float delayBeforeLoad = 1.5f;

    private bool playerInRange = false;
    private bool isOpened = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!playerInRange) return;
        if (isOpened) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PasswordUI.Instance?.OpenUI(this);
        }
    }

    public bool CheckPassword(string input)
    {
        return input == correctPassword;
    }

    public void OpenDoor()
    {
        if (isOpened) return;

        isOpened = true;

        if (DoorClose2 != null)
            DoorClose2.SetActive(false);

        if (DoorOpen2 != null)
            DoorOpen2.SetActive(true);

     

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

          
        }
    }
}