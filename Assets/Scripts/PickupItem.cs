using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Sprite itemIcon;
    public GameObject promptUI;

    public void PickUp()
    {
        Debug.Log("PickUp called on: " + gameObject.name);

        if (ToolBarUI.Instance != null)
        {
            ToolBarUI.Instance.AddItem(itemIcon);
        }

        Destroy(gameObject);
    }

    public void ShowPrompt(bool show)
    {
        if (promptUI != null)
        {
            promptUI.SetActive(show);
        }
    }
}