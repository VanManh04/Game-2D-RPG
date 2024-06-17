using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _targer = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_targer);

                //inventory get wearpon call item effect
                Inventory.instance.GetEquipment(EquipmentType.Weapon).ExecuteItemEffect();
                //hit.GetComponent<Enemy>().Damage();
                //hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());

                //Debug.Log(player.stats.damage.GetValue());
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
