using UnityEngine;
using Highlighting;
public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    public GameObject promptUI;

    [TextArea(2, 4)]
    public string[] pickupDialogue;

    public void PickUp()
    {
        Debug.Log("PickUp called on: " + gameObject.name);

        if (ToolBarUI.Instance != null)
        {
            bool added = ToolBarUI.Instance.AddItem(itemData);

            if (added)
            {
                if (pickupDialogue != null && pickupDialogue.Length > 0)
                {
//                    TutorialDialogueManager.instance.StartDialogue(pickupDialogue);
                }

                var h = GetComponentInParent<Highlightable>();
                if (h != null) h.MarkTaken();

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