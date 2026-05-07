using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string requiredItemID;
    public UnityEvent onSuccess;
    private bool hasBeenUsed = false;

    public bool InteractWith(ItemData item)
    {
        if (hasBeenUsed)
            return false;

        if (item.itemID == requiredItemID)
        {
            hasBeenUsed = true;
            onSuccess?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
}