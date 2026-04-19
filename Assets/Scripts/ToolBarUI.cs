using UnityEngine;

public class ToolBarUI : MonoBehaviour
{
    public static ToolBarUI Instance;

    public ToolSlotUI[] slots;

    private void Awake()
    {
        Instance = this;
    }

    public bool AddItem(Sprite itemIcon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].SetItem(itemIcon);
                return true;
            }
        }

        Debug.Log("Tool is full!");
        return false;
    }
}