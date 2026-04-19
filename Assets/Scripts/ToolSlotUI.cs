using UnityEngine;
using UnityEngine.UI;
public class ToolSlotUI : MonoBehaviour
{
    public Image itemIcon;

    public void SetItem(Sprite iconSprite)
    {
        itemIcon.sprite = iconSprite;
        itemIcon.enabled = true;
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
    }

    public bool IsEmpty()
    {
        return !itemIcon.enabled;
    }
}