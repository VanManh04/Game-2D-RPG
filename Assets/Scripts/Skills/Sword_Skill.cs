using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefabs;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] protected float swordGravity;
}
