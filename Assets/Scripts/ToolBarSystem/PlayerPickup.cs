using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PickupItem currentItem;

    void Update()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentItem.PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null)
        {
            currentItem = item;
            item.ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null && currentItem == item)
        {
            item.ShowPrompt(false);
            currentItem = null;
        }
    }
}