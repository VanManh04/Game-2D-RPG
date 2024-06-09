using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

//ItemData: DaylightTime la 1 ScriptableObject dung de chua thong tin ve cac vat pham trong tro choi
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}