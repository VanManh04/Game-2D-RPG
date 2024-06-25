using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Lever details")]
    [SerializeField] private int lever = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyLeverModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLeverModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelgence);
        Modify(vitality);
        
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < lever; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        //enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
