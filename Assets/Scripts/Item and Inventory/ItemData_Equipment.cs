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

    [Header("Unique eddect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength; //1 point increase damage by 1 and crit.power by 1% (1? -> damage+1, chi mang +1)
    public int agility; //1 point increase evasion by 1% and crit.chance by 1% (1 diem tang ne tranh them 1% va co hoi chi mang len 1%)
    public int intelligence;// 1 point increase magic damage by 1 and magic resistance by 3  (1 diem tang sat thuong them 1 va khang phep them 3 3)
    public int vitality;//1 point increase health by 3 or 5 points (1 diem tang mau tu 3 - 5 don vi)

    [Header("Offensive stats")]
    public int damage;     //tong sat thuong
    public int critChance;     //ti le chi mang
    public int critPower;          //Default value = 150 % ( suc manh chi mang gia tri mac dinh = 150% )

    [Header("Defensive stats")]
    public int health;      //mau toi da
    public int armor;      //giap
    public int evasion;      //ne tranh
    public int magicResistance;      //khang phep

    [Header("Magic stats")]
    public int fireDamage;     //sat thuong lua
    public int iceDamage;      //sat thuong bang
    public int lightingDamage; //sat thuong shock

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerState = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerState.strength.AddModifier(strength);
        playerState.agility.AddModifier(agility);
        playerState.intelgence.AddModifier(intelligence);
        playerState.vitality.AddModifier(vitality);

        playerState.damage.AddModifier(damage);
        playerState.critChance.AddModifier(critChance);
        playerState.critPower.AddModifier(critPower);

        playerState.maxHealth.AddModifier(health);
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
        playerState.intelgence.RemoveModifier(intelligence);
        playerState.vitality.RemoveModifier(vitality);

        playerState.damage.RemoveModifier(damage);
        playerState.critChance.RemoveModifier(critChance);
        playerState.critPower.RemoveModifier(critPower);

        playerState.maxHealth.RemoveModifier(health);
        playerState.armor.RemoveModifier(armor);
        playerState.evasion.RemoveModifier(evasion);
        playerState.magicResistance.RemoveModifier(magicResistance);

        playerState.fireDamage.RemoveModifier(fireDamage);
        playerState.iceDamage.RemoveModifier(iceDamage);
        playerState.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire damage");
        AddItemDescription(iceDamage, "Ice damage");
        AddItemDescription(lightingDamage, "Lighting dmg. ");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (_value > 0)
                sb.Append("+ " + _value + " " + _name + ".");

            descriptionLength++;
        }
    }
}
