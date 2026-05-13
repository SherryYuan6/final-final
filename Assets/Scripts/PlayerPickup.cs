using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPickup : MonoBehaviour
{
    private PickupItem currentItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("E pressed");

            if (currentItem != null)
            {
                //Debug.Log("Picking up item: " + currentItem.name);
                currentItem.PickUp();
            }
            else
            {
                ToolBarUI.Instance?.UseSelectedItem();
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                {
                    ToolBarUI.Instance?.UseSelectedItem(hit.collider.gameObject);
                    //Debug.Log("Trying to use selected toolbar item");

                    //if (ToolBarUI.Instance != null)
                    //{
                    //    ToolBarUI.Instance.UseSelectedItem();
                    //}
                    //else
                    //{
                    //    Debug.LogWarning("ToolBarUI.Instance is null");
                    //}
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
            //item.ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();

        if (item != null && currentItem == item)
        {
            //item.ShowPrompt(false);
            currentItem = null;
        }
    }
}