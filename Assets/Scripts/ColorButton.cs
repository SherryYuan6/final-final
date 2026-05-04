using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string colorID;

    public Image buttonImage;
    public Sprite hoverSprite;

    public ColorSequenceLock lockManager;

    private Sprite normalSprite;

    void Start()
    {
        if (buttonImage != null)
            normalSprite = buttonImage.sprite;
    }

    public void OnClickButton()
    {
        if (lockManager != null)
            lockManager.PressColor(colorID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.sprite = normalSprite;
    }
}