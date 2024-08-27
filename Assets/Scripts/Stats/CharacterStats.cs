using System.Collections;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; //1 point increase damage by 1 and crit.power by 1% (1đ -> damage+1, chi mang +1)
    public Stat agility; //1 point increase evasion by 1% and crit.chance by 1% (1 diem tang ne tranh them 1% va co hoi chi mang len 1%)
    public Stat intelgence;// 1 point increase magic damage by 1 and magic resistance by 3  (1 diem tang sat thuong them 1 va khang phep them 3 3)
    public Stat vitality;//1 point increase health by 5 points (1 diem tang mau tu 5 don vi)

    [Header("Offensive stats")]
    public Stat damage;     //tong sat thuong
    public Stat critChance;     //ti le chi mang
    public Stat critPower;          //Default value = 150 % ( suc manh chi mang gia tri mac dinh = 150% )

    [Header("Defensive stats")]
    public Stat maxHealth;      //mau toi da
    public Stat armor;      //giap
    public Stat evasion;      //ne tranh
    public Stat magicResistance;      //khang phep

    [Header("Magic stats")]
    public Stat fireDamage;     //sat thuong lua
    public Stat iceDamage;      //sat thuong bang
    public Stat lightingDamage; //sat thuong shock

    public bool isIgnited;      // does damage over time "hieu ung dot chay gay sat thuong theo thoi gian"
    public bool isChilled;      // reduce armor by 20%  "hieu ung lam lanh giam giap 20%"
    public bool isShocked;      // reduce accuracy by 20%   "hieu ung sock giam chinh xac 20%"

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;     // bo dem thoi gian hieu ung dot chay
    private float chilledTimer;     // bo dem thoi gian hieu ung lam lanh
    private float shockedTimer;     // bo dem thoi gian hieu ung shock


    private float igniteDamageCooldown = .3f;       // thoi gian giua cac lan gaay sat thuong dot chay
    private float igniteDamageTimer;        // bo dem thoi gian gay sat thuong do dot chay
    private int igniteDamage;       // sat thuong do hieu ung dot chay
    [SerializeField] private GameObject shockStrikePrefabs;
    private int shockDamage;       // sat thuong do hieu ung tia set 

    public int currentHealth;       // mau hien tai

    public System.Action onHealthChanged;       // su kien khi mau thay doi
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; } //bat tu trong Dash
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);     // default chi mang
        currentHealth = GetMaxHealthValue();    // khoi tao mau toi da
        fx = GetComponent<EntityFX>();
        //Debug.Log("CharacterStats Call");
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCorutine(_duration));

    private IEnumerator VulnerableForCorutine(float _duartion)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duartion);

        isVulnerable = false;
    }

    //add 1 cai gi do sau vai giay thi xoa vi du nhu damage
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModifly)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModifly));
    }

    private IEnumerator StatModCoroutine(int modifier, float duration, Stat statToModifly)
    {
        statToModifly.AddModifier(modifier);
        yield return new WaitForSeconds(duration);
        statToModifly.RemoveModifier(modifier);
    }

    //gay sat thuong vat ly cho muc tieu
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool critycalStrike = false;

        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;// muc tieu trang duoc don tan cong -> thoat ham

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();// tong sat thuong = sat thuong co ban + suc manh

        if (CanCrit())
        {
            //Debug.Log("CRIT HIT");

            totalDamage = CalculateCritucalDamage(totalDamage);// Tinh toan sat thuong chi mang neu co the chi mang
            critycalStrike = true;
            //Debug.Log("Total crit damage is "+totalDamage);
        }

        fx.CreateHitFx(_targetStats.transform, critycalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);// tru giap cua muc tieu tu tong sat thuong
        _targetStats.TakeDamage(totalDamage);

        //if invnteroy current weapon has fire effect
        // then
        DoMagicalDamage(_targetStats);// xoa neu khong muon su dung phep thuat
    }

    public virtual void DoDamagePhanTram(CharacterStats _targetStats, int PhanTram)
    {
        bool critycalStrike = false;

        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;// muc tieu trang duoc don tan cong -> thoat ham

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = (damage.GetValue() + strength.GetValue()) * PhanTram / 100;// tong sat thuong = sat thuong co ban + suc manh

        if (CanCrit())
        {
            //Debug.Log("CRIT HIT");

            totalDamage = CalculateCritucalDamage(totalDamage);// Tinh toan sat thuong chi mang neu co the chi mang
            critycalStrike = true;
            //Debug.Log("Total crit damage is "+totalDamage);
        }

        fx.CreateHitFx(_targetStats.transform, critycalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);// tru giap cua muc tieu tu tong sat thuong
        _targetStats.TakeDamage(totalDamage);

        //if invnteroy current weapon has fire effect
        // then
        DoMagicalDamage(_targetStats);// xoa neu khong muon su dung phep thuat
    }

    #region Magical damage and ailemnts
    //gay sat thuong phep len muc tieu va ap dung cac hieu ung phep thuat
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue(); // lay gia tri sat thuong lua
        int _iceDamage = iceDamage.GetValue(); // lay gia tri sat thuong bang
        int _lightingDamage = lightingDamage.GetValue(); // lay gia tri sat thuong set

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelgence.GetValue(); // tong sat thuong phep

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage); // tru khang phep cua muc tieu tu tong sat thuong phep
        _targetStats.TakeDamage(totalMagicalDamage); // Gay sat thuong phep len muc tieu


        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return; // neu khong co sat thuong thi thoat ham

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage; // kiem tra ap dung hieu ung dot chay
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage; // kiem tra ap dung hieu ung bang
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage; // kiem tra ap dung hieu ung shock

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // ap dung hieu ung
                Debug.Log("Applied fire");
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // ap dung hieu ung
                Debug.Log("Applied ice");
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // ap dung hieu ung
                Debug.Log("Applied lighting");
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // thiet lap sat thuong dot chay

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f)); //damage set

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // ap dung hieu ung
    }

    //Ap dung hieu ung dot chay,lam lanh ,sock len nhan vat
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFXFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPrecentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPrecentage, ailmentsDuration);
            fx.ChillFXFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                //find closest target, only among the enemies
                //instatnitate thinder strike
                //setup thunder strike
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
        //isIgnited = _ignite;
        //isChilled = _chill;
        //isShocked = _shock;
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        shockedTimer = ailmentsDuration;
        isShocked = _shock;

        fx.ShockFXFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)       //delete if you dont want shocked target to be hit by shock strike
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefabs, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            // Debug.Log("Take burn damage " + igniteDamage);
            DecreaseHealthBy(igniteDamage);     // gay sat thuong do dot chay

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;       // dat lai thoi gian cho gay sat thuong do dot chay
        }
    }

    //thiet lap gia tri sat thuowng cho hieu ung dot chay
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    //Gay sat thuong cho nhan vat kiem tra xem nhan vat co chet khong
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        // Debug.Log(_damage);

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        //cap nhat UI
        if (onHealthChanged != null)
            onHealthChanged();
    }

    //Giam mau hien tai cua nhan vat
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        Debug.Log("Die");
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stay caculations
    //kiem tra giap cua muc tieu va giam sat thuong tuong ung
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f); // muc tieu bi lam lanh thi giam giap cua muc tieu
        else
            totalDamage -= _targetStats.armor.GetValue(); // giam giap cua muc tieu tu tong sat thuong

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // gioi han sat thuong khong nho hon 0
        return totalDamage;
    }

    //kiem tra khang phep cua muc tieu va dieu chinh tong sat thuong phep duwa tren khang phep va tri tue cua muc tieu
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // Tru khang phep cua muc tieu va chi so tri tue nhan 3 tu tong sat thuong phep
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelgence.GetValue() * 3);

        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);// gioi han tong sat thuong phep khong nho hon 0
        return totalMagicalDamage;// tra ve tong sat thuong phep
    }

    public virtual void OnEvasion()
    {

    }

    //kiem tra muc tieu xem co the ne tranh don tan cong khong
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();// tong ne tranh = ne tranh co ban + nhanh nhen

        if (isShocked)
            totalEvasion += 20;// nhan vat bi shock thi tang ne tranh cua muc tieu

        if (Random.Range(0, 100) < totalEvasion)
        {
            //Debug.Log("ATTACK AVOIDED");
            _targetStats.OnEvasion();
            return true;// don tan cong bi tranh
        }

        return false;// don tan cong danh trung
    }


    //Kiem tra don tam cong co the gay sat thuong chi mang hay khon
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();// tong co hoi chi mang = co hoi chi mang co ban + nhanh nhen

        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;//co the chi mang

        return false;
    }



    //Tinh toan sat thuong chi mang
    protected int CalculateCritucalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;// tinh tong suc manh chi mang

        float critDamage = _damage * totalCritPower;// tinh sat thuong chi mang

        //Debug.Log("Total crit power % " + totalCritPower);
        //Debug.Log("crir damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }


    //Tinh toan tra ve gia tri toi da cua mau
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    public Stat GetStat(StatType _statsType)
    {
        if (_statsType == StatType.strength)
            return strength;
        else if (_statsType == StatType.agility)
            return agility;
        else if (_statsType == StatType.intelegence)
            return intelgence;
        else if (_statsType == StatType.vitality)
            return vitality;
        else if (_statsType == StatType.damage)
            return damage;
        else if (_statsType == StatType.critChance)
            return critChance;
        else if (_statsType == StatType.critPower)
            return critPower;
        else if (_statsType == StatType.health)
            return maxHealth;
        else if (_statsType == StatType.armor)
            return armor;
        else if (_statsType == StatType.evasion)
            return evasion;
        else if (_statsType == StatType.magicRes)
            return magicResistance;
        else if (_statsType == StatType.fireDamage)
            return fireDamage;
        else if (_statsType == StatType.iceDamage)
            return iceDamage;
        else if (_statsType == StatType.lightingDamage)
            return lightingDamage;

        return null;
    }
}
