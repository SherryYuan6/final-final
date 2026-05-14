using UnityEngine;

public class BreakableCamera : MonoBehaviour
{
    public string requiredItemID = "Axe";
    public GameObject normalCameraModel;
    public GameObject brokenCameraModel;
    public AudioSource breakSound;
    public ParticleSystem sparkEffect;

    [SerializeField] float breakRange = 3f;

    private bool isBroken;
    private bool playerInRange;
    private Transform player;

    void Start()
    {
        SetModel(false);

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (isBroken || player == null) return;

        // E TO BREAK
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryBreak();
        }
    }

    public void TryBreak()
    {
        if (isBroken || player == null) return;

        if (Vector3.Distance(transform.position, player.position) > breakRange)
            return;

        ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();

        if (selectedItem != null && selectedItem.itemID == requiredItemID)
        {
            BreakCam();

            if (selectedItem.consumeOnUse)
                ToolBarUI.Instance.RemoveItems(requiredItemID, 1);
        }
    }

    void BreakCam()
    {
        isBroken = true;

        GetComponent<SecurityCamera>()?.Disable();
        SetModel(true);

        if (breakSound != null) breakSound.Play();
        if (sparkEffect != null) sparkEffect.Play();

        TutorialDialogueManager.instance.StartDialogue(new string[]
        {
            "Good. No more eyes on me.",
            "The light still looks wrong. The shadows are in the wrong place.",
            "I have been seeing things. Small things. A texture that does not resolve.",
            "There is something in this building I need to find.",
        });
    }

    private void SetModel(bool broken)
    {
        normalCameraModel?.SetActive(!broken);
        brokenCameraModel?.SetActive(broken);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}