using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefabs;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] protected float swordGravity;

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefabs, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(launchDir, swordGravity);
    }
}
