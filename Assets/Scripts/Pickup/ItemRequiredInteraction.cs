using UnityEngine;

public class ItemRequiredInteraction : MonoBehaviour
{
    public string requiredItemID;
    public GameObject promptUI;

    private bool playerInRange = false;
    private bool used = false;

    void Update()
    {
        if (!playerInRange || used || !Input.GetKeyDown(KeyCode.E))
            return;
        ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();
        
        if (selectedItem != null && selectedItem.itemID == requiredItemID)
            UseItem();
    }

    void UseItem()
    {
        used = true;
        //Debug.Log("Used item: " + item.itemID);

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        gameObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptUI != null)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }
}