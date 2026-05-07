using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public Sprite Icon;
    
    public bool isConsumable;
    public float cognitiveRestoreAmount;
    
    public bool consumeOnUse = true;
}

