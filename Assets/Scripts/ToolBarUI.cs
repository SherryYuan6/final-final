using UnityEngine;

public class ToolBarUI : MonoBehaviour
{
    public static ToolBarUI Instance;

    public ToolSlotUI[] slots;
    public int selectedIndex = -1;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(i);
        }
    }

    public bool AddItem(ItemData itemData)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].SetItem(itemData);

                if (selectedIndex == -1)
                {
                    selectedIndex = i;
                }

                return true;
            }
        }

        Debug.Log("Tool is full!");
        return false;
    }

    public ItemData GetSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= slots.Length)
            return null;

        return slots[selectedIndex].GetItem();
    }

public void UseSelectedItem(GameObject target = null)
{
    Debug.Log("UseSelectedItem called");

    ItemData selectedItem = GetSelectedItem();

    if (selectedItem == null)
    {
        Debug.Log("No selected item.");
        return;
    }

    Debug.Log("Selected item ID: " + selectedItem.itemID);

    if (selectedItem.itemID == "pills")
    {
        if (ChipManager.Instance != null)
        {
            ChipManager.Instance.AddCognitive(selectedItem.cognitiveRestoreAmount);
            Debug.Log("Used pills. Cognitive +" + selectedItem.cognitiveRestoreAmount);
        }
        else
        {
            Debug.LogWarning("Decay.Instance is null");
            return;
        }

        RemoveSelectedItem();
    }
    else
    {
        Debug.Log("Selected item is not usable: " + selectedItem.itemID);
        UseItemOnTarget(selectedItem, target);
        }
    }

    private void UseItemOnTarget(ItemData item, GameObject target)
    {
        if (target == null)
        {
            Debug.Log("No target clicked.");
            return;
        }

        Interactable interactable = target.GetComponent<Interactable>();
        if (interactable != null)
        {
            bool success = interactable.InteractWith(item);
            if (success && item.consumeOnUse)
                RemoveSelectedItem();
        }
        else
        {
            Debug.Log(target.name + " is not interactable.");
        }
    }


    public void RemoveSelectedItem()
{
    if (selectedIndex < 0 || selectedIndex >= slots.Length)
    {
        Debug.LogWarning("Invalid selectedIndex: " + selectedIndex);
        return;
    }

    slots[selectedIndex].ClearSlot();

    Debug.Log("Removed selected item from slot: " + selectedIndex);
}
    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        selectedIndex = index;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == selectedIndex);
        }

        Debug.Log("Selected slot: " + index);
    }

    public int CountItem(string itemID)
    {
        int count = 0;

        foreach (ToolSlotUI slot in slots)
        {
            ItemData item = slot.GetItem();

            if (item != null && item.itemID == itemID)
            {
                count++;
            }
        }

        return count;
    }

    public void RemoveItems(string itemID, int amount)
    {
        int removed = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (removed >= amount)
                return;

            ItemData item = slots[i].GetItem();

            if (item != null && item.itemID == itemID)
            {
                slots[i].ClearSlot();
                removed++;
            }
        }
    }
}