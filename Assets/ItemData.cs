using System.Globalization;
using UnityEngine;

//ItemData: DaylightTime la 1 ScriptableObject dung de chua thong tin ve cac vat pham trong tro choi
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}