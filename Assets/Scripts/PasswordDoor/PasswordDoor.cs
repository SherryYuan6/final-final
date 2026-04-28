using UnityEngine;

public class PasswordDoor : MonoBehaviour
{
    [Header("Password Settings")]
    public string correctPassword = " ";

    [Header("Door Models")]
    public GameObject DoorClose2;
    public GameObject DoorOpen2;

    [Header("UI")]
    public GameObject promptUI;

    [Header("Player Check")]
    public bool playerInRange = false;

    private bool isOpened = false;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            if (PasswordUI.Instance != null)
            {
                PasswordUI.Instance.OpenUI(this);
            }
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

        if (promptUI != null)
            promptUI.SetActive(false);

        Debug.Log("Door opened!");
    }

    public void ShowPrompt(bool show)
    {
        if (promptUI != null && !isOpened)
        {
            promptUI.SetActive(show);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ShowPrompt(false);
        }
    }
}