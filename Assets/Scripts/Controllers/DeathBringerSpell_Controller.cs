using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask WhatIsPlayer;

    private CharacterStats myStats;

    public void SetupSpell(CharacterStats _stats) => myStats = _stats;

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, WhatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
                Debug.Log("Player damage");

                //hit.GetComponent<Player>()?.fx.ScreenShake(new Vector3(2, 2));
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestruy() => Destroy(gameObject);
}
