using UnityEngine;

public class BreakableCamera : MonoBehaviour
{
    public string requiredItemID = "hammer";

    public GameObject promptUI;
    public GameObject normalCameraModel;
    public GameObject brokenCameraModel;

    //remember find the audio
    public AudioSource breakSound;
    public ParticleSystem sparkEffect;

    private bool playerInRange = false;
    private bool isBroken = false;

    void Start()
    {
        if (normalCameraModel != null)
            normalCameraModel.SetActive(true);

        if (brokenCameraModel != null)
            brokenCameraModel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isBroken && Input.GetKeyDown(KeyCode.E))
        {
            ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();

            if (selectedItem != null && selectedItem.itemID == requiredItemID)
            {
                BreakCamera();
            }
            else
            {
                Debug.Log("You need to select the correct item.");
            }
        }
    }

    void BreakCamera()
    {
        isBroken = true;

        Debug.Log("Camera broken!");

        if (normalCameraModel != null)
            normalCameraModel.SetActive(false);

        if (brokenCameraModel != null)
            brokenCameraModel.SetActive(true);

        if (breakSound != null)
            breakSound.Play();

        if (sparkEffect != null)
            sparkEffect.Play();

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Something entered trigger: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Player entered camera range");
        playerInRange = true;

        if (promptUI != null && !isBroken)
            promptUI.SetActive(true);
    }
}

private void OnTriggerExit(Collider other)
{
    Debug.Log("Something exited trigger: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Player left camera range");
        playerInRange = false;

        if (promptUI != null)
            promptUI.SetActive(false);
    }
}
}