using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PickupItem currentItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");

            if (currentItem != null)
            {
                Debug.Log("Picking up item: " + currentItem.name);
                currentItem.PickUp();
            }
            else
            {
                Debug.Log("Trying to use selected toolbar item");

                if (ToolBarUI.Instance != null)
                {
                    ToolBarUI.Instance.UseSelectedItem();
                }
                else
                {
                    Debug.LogWarning("ToolBarUI.Instance is null");
                }
            }
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