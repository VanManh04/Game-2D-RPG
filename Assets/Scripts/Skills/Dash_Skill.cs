using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockedButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Created clone behind");
    }

    private void UnlockDash()
    {
        //Debug.Log("Attemtpt to lock dash");
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        //Debug.Log("Attemtpt to lock dash");
        if(cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        //Debug.Log("Attemtpt to lock dash");
        if (cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }


    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

}