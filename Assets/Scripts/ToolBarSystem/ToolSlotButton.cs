using UnityEngine;
using UnityEngine.EventSystems;

public class ToolSlotButton : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolBarUI.Instance != null)
        {
            ToolBarUI.Instance.SelectSlot(slotIndex);
        }
    }
}