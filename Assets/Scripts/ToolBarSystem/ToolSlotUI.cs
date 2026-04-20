using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ToolSlotUI : MonoBehaviour
{
    public Image itemIcon;
    private ItemData currentItem;
    public GameObject highlight;
    public void SetItem(ItemData item)
    {
        currentItem = item;
        itemIcon.sprite = item.Icon;
        itemIcon.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public ItemData GetItem()
    {
        return currentItem;
    }

    public void SetSelected(bool selected)
    {
        if (highlight != null)
        {
            highlight.SetActive(selected);
        }
    }
}
    