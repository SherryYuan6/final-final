using UnityEngine;

public class MechanicalArm : MonoBehaviour
{
    public string requiredItemID = "gear";

    public GameObject promptUI;
    public GameObject normalMechanicalArm;
    public GameObject changeMechanicalArm;
    public AudioSource sound;

    private bool playerInRange = false;
    private bool isChanged = false;

    void Start()
    {
        if (normalMechanicalArm != null)
            normalMechanicalArm.SetActive(true);

        if (changeMechanicalArm != null)
            changeMechanicalArm.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isChanged && Input.GetKeyDown(KeyCode.E))
        {
            ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();

            if (selectedItem != null && selectedItem.itemID == requiredItemID)
            {
                ChangeArm();
            }
            else
            {
                Debug.Log("You need a gear.");
            }
        }
    }

    void ChangeArm()
    {
        isChanged = true;

        if (normalMechanicalArm != null)
            normalMechanicalArm.SetActive(false);

        if (changeMechanicalArm != null)
            changeMechanicalArm.SetActive(true);

        if (sound != null)
            sound.Play();

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChanged)
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}