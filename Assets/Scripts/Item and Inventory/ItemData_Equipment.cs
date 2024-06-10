using System.Collections.Generic;
using UnityEngine;
//EquipmentType dinh nghia cac loai trang bi co the co trong tro choi,bao gom Weapon(vu khi), Armor(Giap),Amulet,Flask(Binh thuoc)
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major stats")]
    public int strength; //1 point increase damage by 1 and crit.power by 1% (1? -> damage+1, chi mang +1)
    public int agility; //1 point increase evasion by 1% and crit.chance by 1% (1 diem tang ne tranh them 1% va co hoi chi mang len 1%)
    public int intelgence;// 1 point increase magic damage by 1 and magic resistance by 3  (1 diem tang sat thuong them 1 va khang phep them 3 3)
    public int vitality;//1 point increase health by 3 or 5 points (1 diem tang mau tu 3 - 5 don vi)

    [Header("Offensive stats")]
    public int damage;     //tong sat thuong
    public int critChance;     //ti le chi mang
    public int critPower;          //Default value = 150 % ( suc manh chi mang gia tri mac dinh = 150% )

    [Header("Defensive stats")]
    public int maxHealth;      //mau toi da
    public int armor;      //giap
    public int evasion;      //ne tranh
    public int magicResistance;      //khang phep

    [Header("Magic stats")]
    public int fireDamage;     //sat thuong lua
    public int iceDamage;      //sat thuong bang
    public int lightingDamage; //sat thuong shock

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void AddModifiers()
    {
        PlayerStats playerState = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerState.strength.AddModifier(strength);
        playerState.agility.AddModifier(agility);
        playerState.intelgence.AddModifier(intelgence);
        playerState.vitality.AddModifier(vitality);

        playerState.damage.AddModifier(damage);
        playerState.critChance.AddModifier(critChance);
        playerState.critPower.AddModifier(critPower);

        playerState.maxHealth.AddModifier(maxHealth);
        playerState.armor.AddModifier(armor);
        playerState.evasion.AddModifier(evasion);
        playerState.magicResistance.AddModifier(magicResistance);

        playerState.fireDamage.AddModifier(fireDamage);
        playerState.iceDamage.AddModifier(iceDamage);
        playerState.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerState = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerState.strength.RemoveModifier(strength);
        playerState.agility.RemoveModifier(agility);
        playerState.intelgence.RemoveModifier(intelgence);
        playerState.vitality.RemoveModifier(vitality);

        playerState.damage.RemoveModifier(damage);
        playerState.critChance.RemoveModifier(critChance);
        playerState.critPower.RemoveModifier(critPower);

        playerState.maxHealth.RemoveModifier(maxHealth);
        playerState.armor.RemoveModifier(armor);
        playerState.evasion.RemoveModifier(evasion);
        playerState.magicResistance.RemoveModifier(magicResistance);

        playerState.fireDamage.RemoveModifier(fireDamage);
        playerState.iceDamage.RemoveModifier(iceDamage);
        playerState.lightingDamage.RemoveModifier(lightingDamage);
    }
}
