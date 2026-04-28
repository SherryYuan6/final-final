using UnityEngine;

public class ShelfInteract : MonoBehaviour
{
    public LockBoxDialPassword lockUI;
    public GameObject promptUI;

    private bool playerInRange = false;

    void Start()
    {
        Debug.Log("ShelfInteract script is running on: " + gameObject.name);

        if (promptUI != null)
            promptUI.SetActive(false);
        else
            Debug.LogWarning("Prompt UI is missing!");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed near lock box.");

            if (lockUI != null)
                lockUI.OpenLockUI();
            else
                Debug.LogError("Lock UI is missing!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered 3D trigger: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered lock box range.");
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited 3D trigger: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left lock box range.");
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}