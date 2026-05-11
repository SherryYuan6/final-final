using UnityEngine;
using UnityEngine.EventSystems;

public class NumberBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int number;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform startParent;
    private Vector2 startPosition;

    public Transform originalParent;
    public Vector2 originalPosition;
    public DropSlot originalSlot;
    

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startParent = transform.parent;
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        originalSlot = GetComponentInParent<DropSlot>();

        if (originalSlot != null)
        {
            originalSlot.ClearSlot();
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            ReturnToOriginalPlace();
        }
    }

    public void ReturnToOriginalPlace()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;

        if (originalSlot != null)
        {
            originalSlot.SetBlock(this);
        }
    }

    public void ResetToStart()
{
    transform.SetParent(startParent);
    rectTransform.anchoredPosition = startPosition;
}
}