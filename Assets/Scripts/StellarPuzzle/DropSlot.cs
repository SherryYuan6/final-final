using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int correctNumber;
    public NumberPuzzleManager manager;

    private NumberBlock currentBlock;

    public void OnDrop(PointerEventData eventData)
    {
        NumberBlock newBlock = eventData.pointerDrag.GetComponent<NumberBlock>();

        if (newBlock == null)
            return;

        if (currentBlock != null)
        {
            NumberBlock oldBlock = currentBlock;

            oldBlock.transform.SetParent(newBlock.originalParent);
            oldBlock.GetComponent<RectTransform>().anchoredPosition = newBlock.originalPosition;

            if (newBlock.originalSlot != null)
            {
                newBlock.originalSlot.SetBlock(oldBlock);
            }
        }

        SetBlock(newBlock);

        if (manager != null)
        {
            manager.CheckAnswer();
        }
    }

    public void SetBlock(NumberBlock block)
    {
        currentBlock = block;
        block.transform.SetParent(transform);
        block.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ClearSlot()
    {
        currentBlock = null;
    }

    public bool IsCorrect()
    {
        return currentBlock != null && currentBlock.number == correctNumber;
    }

    public bool HasBlock()
    {
        return currentBlock != null;
    }
}