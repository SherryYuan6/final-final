using UnityEngine;

public class ToolBarUI : MonoBehaviour
{
    public static ToolBarUI Instance;

    public ToolSlotUI[] slots;
    public int selectedIndex = -1;

    private void Awake()
    {
        Instance = this;
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