using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    public GameObject promptUI;

    public void PickUp()
    {
        Debug.Log("PickUp called on: " + gameObject.name);

        if (ToolBarUI.Instance != null)
        {
            bool added = ToolBarUI.Instance.AddItem(itemData);

            if (added)
            {
                TutorialDialogueManager.instance.StartDialogue(new string[]
            {
                "Good.",
                "Now find a camera.",
                "Press E near it to smash it."
            });
                Destroy(gameObject);
            }
        }
    }

    public void ShowPrompt(bool show)
    {
        if (promptUI != null)
        {
            promptUI.SetActive(show);
        }
    }

}