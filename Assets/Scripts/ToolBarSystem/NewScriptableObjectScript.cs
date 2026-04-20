using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Tool/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public Sprite Icon;
}
