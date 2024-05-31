using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; //1 point increase damage by 1 and crit.power by 1% (1đ -> damage+1, chí mạng +1)
    public Stat agility; //1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelgence;// 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;//1 point increase health by 3 or 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;          //Default value = 150 %

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;


    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            //Debug.Log("CRIT HIT");

            totalDamage = CalculateCritucalDamage(totalDamage);

            //Debug.Log("Total crit damage is "+totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log("Die");
        //throw new NotImplementedException();
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);//giới hạn health 
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            //Debug.Log("ATTACK AVOIDED");
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;

        return false;
    }

    private int CalculateCritucalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        //Debug.Log("Total crit power % " + totalCritPower);
        //Debug.Log("crir damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }
}
