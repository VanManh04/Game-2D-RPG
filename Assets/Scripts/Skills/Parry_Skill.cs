using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealPerentage;
    public bool restoreUnlock { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlock { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlock)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealPerentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    public void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlock = true;
    }

    public void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlock = true;
    }

    public void MakeMirrageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlock)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
