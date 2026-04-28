using UnityEngine;

public class MechanicalArm : MonoBehaviour
{
    public string requiredItemID = "gear";
    public int requiredAmount = 3;

    public GameObject promptUI;
    public GameObject normalMechanicalArm;
    public GameObject changeMechanicalArm;

    public GameObject hiddenPoster; // 机械臂启动后出现的海报

    public AudioSource sound;

    private bool playerInRange = false;
    private bool isActivated = false;
    private bool hasShownHint = false;

    void Start()
    {
        normalMechanicalArm.SetActive(true);
        changeMechanicalArm.SetActive(false);

        if (hiddenPoster != null)
            hiddenPoster.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isActivated) return;

            int count = ToolBarUI.Instance.CountItem(requiredItemID);

            if (count < requiredAmount)
            {
                if (!hasShownHint)
                {
                    hasShownHint = true;

                    TutorialDialogueManager.instance.StartDialogue(new string[]
                    {
                        "Instruction Manual:",
                        "Install three parts to activate it.",
                        "Look around for missing pieces."
                    });
                }
                else
                {
                    TutorialDialogueManager.instance.StartDialogue(new string[]
                    {
                        "Still missing parts."
                    });
                }
            }
            else
            {
                ActivateArm();
            }
        }
    }

    void ActivateArm()
    {
        isActivated = true;

        ToolBarUI.Instance.RemoveItems(requiredItemID, requiredAmount);

        normalMechanicalArm.SetActive(false);
        changeMechanicalArm.SetActive(true);

        if (hiddenPoster != null)
            hiddenPoster.SetActive(true);

        if (sound != null)
            sound.Play();

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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