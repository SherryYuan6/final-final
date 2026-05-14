using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PickupItem currentItem;

    void Update()
    {
        HandleInteract();
    }

    void HandleInteract()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;
        if (currentItem != null)
        {
            currentItem.PickUp();
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TryHitWithAxe();
                return;
            }

            ToolBarUI.Instance?.UseSelectedItem(hit.collider.gameObject);
            return;
        }
        ToolBarUI.Instance?.UseSelectedItem();
    
}

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<PickupItem>();
        if (item != null)
            currentItem = item;
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<PickupItem>();
        if (item != null && currentItem == item)
            currentItem = null;
    }

    private void OnDisable()
    {
        currentItem = null;
    }
}