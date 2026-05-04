using UnityEngine;
public class PlayerPickup : MonoBehaviour
{
    private PickupItem currentItem;

    void Update()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E) && !CursorManager.IsUnlocked)
                currentItem.PickUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item == null)
            return;
            currentItem = item;
            item.ShowPrompt(true);
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item == null || currentItem != item)
            return;
            item.ShowPrompt(false);
            currentItem = null;
    }
}