using System.Text;
using UnityEditor;
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
    public string itemId;

    //random Drop
    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}