using UnityEngine;
//EquipmentType dinh nghia cac loai trang bi co the co trong tro choi,bao gom Weapon(vu khi), Armor(Giap),Amulet,Flask(Binh thuoc)
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
