using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; //1 point increase damage by 1 and crit.power by 1% (1đ -> damage+1, chí mạng +1)
    public Stat agility; //1 point increase evasion by 1% and crit.chance by 1% (1 điểm tăng né tránh thêm 1% và cơ hội chí mạng thêm 1%)
    public Stat intelgence;// 1 point increase magic damage by 1 and magic resistance by 3  (1 điểm tăng sát thương phép thêm 1 và kháng phép thêm 3)
    public Stat vitality;//1 point increase health by 3 or 5 points (1 điểm tăng máu thêm từ 3 đến 5 đơn vị)

    [Header("Offensive stats")]
    public Stat damage;     //Tổng sát thương
    public Stat critChance;     //Tỉ lệ chí mạng
    public Stat critPower;          //Default value = 150 % ( Sức mạnh chí mạng giá trị mặc định = 150% )

    [Header("Defensive stats")]
    public Stat maxHealth;      //Máu tối đa
    public Stat armor;      //Giáp
    public Stat evasion;      //Né tránh 
    public Stat magicResistance;      //Kháng phép

    [Header("Magic stats")]
    public Stat fireDamage;     //Sát thương lửa
    public Stat iceDamage;      //Sát thương băng
    public Stat lightingDamage; //Sát thương sét

    public bool isIgnited;      // does damage over time "Hiệu ứng đốt cháy gây sát thương theo thời gian"
    public bool isChilled;      // reduce armor by 20%  "Hiệu ứng làm lạnh giảm giáp 20%"
    public bool isShocked;      // reduce accuracy by 20%   "Hiệu ứng sốc giảm chính xác 20%"

    private float ignitedTimer;     // Bộ đếm thời gian hiệu ứng đốt cháy
    private float chilledTimer;     // Bộ đếm thời gian hiệu ứng làm lạnh
    private float shockedTimer;     // Bộ đếm thời gian hiệu ứng sốc


    private float igniteDamageCooldown = .3f;       // Thời gian chờ giữa các lần gây sát thương do đốt cháy
    private float igniteDamageTimer;        // Bộ đếm thời gian gây sát thương do đốt cháy
    private int igniteDamage;       // Sát thương do hiệu ứng đốt cháy


    public int currentHealth;       // Máu hiện tại

    public System.Action onHealthChanged;       // Sự kiện khi máu thay đổi

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);     // Đặt giá trị mặc định cho sức mạnh chí mạng
        currentHealth = GetMaxHealthValue();    // Khởi tạo máu hiện tại bằng giá trị máu tối đa

        //Debug.Log("CharacterStats Call");
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;     //thời gian đốt cháy
        chilledTimer -= Time.deltaTime;     //thời gian làm lạnh
        shockedTimer -= Time.deltaTime;     //thời gian sốc

        igniteDamageTimer -= Time.deltaTime;    // Giảm thời gian chờ gây sát thương do đốt cháy

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if(shockedTimer < 0)
            isShocked = false;

        if (igniteDamageTimer < 0 && isIgnited)     // Nếu còn hiệu ứng đốt cháy và thời gian chờ đã hết
        {
            Debug.Log("Take burn damage "+igniteDamage);

            DecreaseHealthBy(igniteDamage);     // Gây sát thương do đốt cháy

            if (currentHealth < 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown;       // Đặt lại thời gian chờ gây sát thương do đốt cháy
        }
    }


    //Gây sát thương vật lý cho mục tiêu
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;// Nếu mục tiêu tránh được đòn tấn công thì thoát hàm

        int totalDamage = damage.GetValue() + strength.GetValue();// Tổng sát thương = sát thương cơ bản + sức mạnh

        if (CanCrit())
        {
            //Debug.Log("CRIT HIT");

            totalDamage = CalculateCritucalDamage(totalDamage);// Tính toán sát thương chí mạng nếu có thể chí mạng

            //Debug.Log("Total crit damage is "+totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);// Trừ giáp của mục tiêu từ tổng sát thương
        //_targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);// Gây sát thương phép
    }


    //Gây sát thương phép lên mục tiêu và áp dụng các hiệu ứng phép thuật.
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue(); // Lấy giá trị sát thương lửa
        int _iceDamage = iceDamage.GetValue(); // Lấy giá trị sát thương băng
        int _lightingDamage = lightingDamage.GetValue(); // Lấy giá trị sát thương sét

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelgence.GetValue(); // Tổng sát thương phép

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage); // Trừ kháng phép của mục tiêu từ tổng sát thương phép
        _targetStats.TakeDamage(totalMagicalDamage); // Gây sát thương phép lên mục tiêu


        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return; // Nếu không có sát thương phép thì thoát hàm

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage; // Kiểm tra có thể áp dụng hiệu ứng đốt cháy
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage; // Kiểm tra có thể áp dụng hiệu ứng băng
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage; // Kiểm tra có thể áp dụng hiệu ứng sốc

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Áp dụng hiệu ứng
                Debug.Log("Applied fire");
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Áp dụng hiệu ứng
                Debug.Log("Applied ice");
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Áp dụng hiệu ứng
                Debug.Log("Applied lighting");
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // Thiết lập sát thương đốt cháy

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Áp dụng hiệu ứng
    }


    //Kiểm tra kháng phép của mục tiêu và điều chỉnh tổng sát thương phép dựa trên kháng phép và trí tuệ của mục tiêu.
    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // Trừ kháng phép của mục tiêu và chỉ số trí tuệ nhân 3 từ tổng sát thương phép
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelgence.GetValue() * 3);

        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);// Giới hạn tổng sát thương phép không nhỏ hơn 0
        return totalMagicalDamage;// Trả về tổng sát thương phép sau khi trừ kháng phép và giới hạn
    }


    //Áp dụng các hiệu ứng đốt cháy, làm lạnh và sốc lên nhân vật.
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 2;
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 2;
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 2;
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }


    //Thiết lập giá trị sát thương cho hiệu ứng đốt cháy.
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;


    //Gây sát thương cho nhân vật và kiểm tra xem nhân vật có chết không.
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        Debug.Log(_damage);

        if (currentHealth <= 0)
            Die();
    }


    //Giảm máu hiện tại của nhân vật.
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        Debug.Log("Die");
        //throw new NotImplementedException();
    }


    //Kiểm tra giáp của mục tiêu và giảm sát thương tương ứng.
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f); // Nếu mục tiêu bị làm lạnh, giảm giáp của mục tiêu
        else
            totalDamage -= _targetStats.armor.GetValue(); // Giảm giáp của mục tiêu từ tổng sát thương

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // Giới hạn sát thương không nhỏ hơn 0
        return totalDamage;
    }


    //Kiểm tra xem mục tiêu có thể né tránh đòn tấn công không.
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();// Tổng né tránh = né tránh cơ bản + nhanh nhẹn

        if (isShocked)
            totalEvasion += 20;// Nếu nhân vật bị sốc, tăng khả năng né tránh của mục tiêu

        if (Random.Range(0, 100) < totalEvasion)
        {
            //Debug.Log("ATTACK AVOIDED");
            return true;// Nếu né tránh lớn hơn số ngẫu nhiên từ 0 đến 100, đòn tấn công bị tránh
        }

        return false;// Đòn tấn công không bị tránh
    }


    //Kiểm tra xem đòn tấn công có thể gây sát thương chí mạng không.
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();// Tổng cơ hội chí mạng = cơ hội chí mạng cơ bản + nhanh nhẹn

        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;// Nếu cơ hội chí mạng lớn hơn hoặc bằng số ngẫu nhiên từ 0 đến 100, có thể chí mạng

        return false;
    }



    //Tính toán sát thương chí mạng.
    private int CalculateCritucalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;// Tính tổng sức mạnh chí mạng

        float critDamage = _damage * totalCritPower;// Tính sát thương chí mạng

        //Debug.Log("Total crit power % " + totalCritPower);
        //Debug.Log("crir damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }


    //Tính toán và trả về giá trị máu tối đa của nhân vật.
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
