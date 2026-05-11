using UnityEngine;

public class BreakableCamera : MonoBehaviour
{
    public string requiredItemID = "Axe";
    public GameObject promptUI;
    public GameObject normalCameraModel;
    public GameObject brokenCameraModel;
    public AudioSource breakSound;
    public ParticleSystem sparkEffect;

    private bool playerInRange;
    private bool isBroken;

    void Start()
    {
        SetModel(broken: false);
    }

    void Update()
    {
        if (isBroken)
            return;
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();
            if (selectedItem != null && selectedItem.itemID == requiredItemID)
            {
                BreakCamera();
                if (selectedItem.consumeOnUse)
                    ToolBarUI.Instance.RemoveItems(requiredItemID, 1);
            }
        }
    }

    void BreakCamera()
    {
        isBroken = true;
        GetComponent<SecurityCamera>()?.Disable();
        //Debug.Log("Camera broken!");
        SetModel(broken: true);

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        if (breakSound != null)
            breakSound.Play();

        if (sparkEffect != null)
            sparkEffect.Play();


        TutorialDialogueManager.instance.StartDialogue(new string[]
        {
           "Good. No more eyes on me.",
            "The light still looks wrong. The shadows are in the wrong place.",
            "I have been seeing things. Small things. A texture that does not resolve. A sound from somewhere below the floor.",
            "I thought it was stress. I am not sure anymore.",
            "There is something in this building I need to find.",
            "I need to keep moving forward, or they'll catch up to me."
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isBroken)
        return;

        playerInRange = true;
        promptUI.SetActive(true);
    }

private void OnTriggerExit(Collider other)
{
    //Debug.Log("Something exited trigger: " + other.name);
    if (!other.CompareTag("Player")) return;
        playerInRange = false;

        if (promptUI != null)
            promptUI.SetActive(false);
}
    private void SetModel(bool broken)
    {
        normalCameraModel?.SetActive(!broken);
        brokenCameraModel?.SetActive(broken);
    }
}