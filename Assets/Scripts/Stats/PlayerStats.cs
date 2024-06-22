using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        //player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        Debug.Log("PLayer avoided attack");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;// muc tieu trang duoc don tan cong -> thoat ham

        int totalDamage = damage.GetValue() + strength.GetValue();// tong sat thuong = sat thuong co ban + suc manh

        if (_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CanCrit())
        {
            //Debug.Log("CRIT HIT");

            totalDamage = CalculateCritucalDamage(totalDamage);// Tinh toan sat thuong chi mang neu co the chi mang

            //Debug.Log("Total crit damage is "+totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);// tru giap cua muc tieu tu tong sat thuong
        _targetStats.TakeDamage(totalDamage);

        //if invnteroy current weapon has fire effect
        // then
        DoMagicalDamage(_targetStats);// xoa neu khong muon su dung phep thuat
    }
}
