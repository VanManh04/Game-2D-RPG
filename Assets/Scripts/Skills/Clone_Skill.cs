using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplivcateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInseadOfClone;

    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition,cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplivcateClone,chanceToDuplicate,player);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWidthDelay(_enemyTransform, new Vector3(1.5f * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWidthDelay(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}