using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Sprite itemIcon;
    public GameObject promptUI;

    public void PickUp()
    {
        bool added = ToolBarUI.Instance.AddItem(itemIcon);

        if (added)
        {
            Destroy(gameObject);
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