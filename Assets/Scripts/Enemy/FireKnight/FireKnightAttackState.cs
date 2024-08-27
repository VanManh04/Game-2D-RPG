using UnityEngine;
//Maus > 75 % (0, 1) 50 50
//mau < 70 > 50(0, 1, 2) 30 30 40
//mau < 50 > 30(0, 1, 2, 3) 15 15 40 30
//mau < 30(0, 1, 2, 3, 4) 10 10 30 10 30

public class FireKnightAttackState : EnemyState
{
    private Enemy_FireKnight enemy;
    private Player player;
    private int hpEnemy,maxHP;
    public int attackNumber { get; private set; }

    public FireKnightAttackState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;
        hpEnemy = enemy.stats.currentHealth;
        maxHP = enemy.stats.maxHealth.GetValue();
        ApplyHPNumberATK();

        for (int i = 0; i < enemy.offSetRadius.Length; i++)
        {
            if (attackNumber == i)
                enemy.SetupAttackCheck(enemy.offSetPlus[i], enemy.offSetRadius[i]);
        }
    }


    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        if (Mathf.Abs(player.transform.position.x - enemy.transform.position.x) < .2f)
        {
            enemy.anim.SetInteger("attackNumber", 1);
        }
        else
            enemy.anim.SetInteger("attackNumber", attackNumber);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    private int PhanTramHP(int phanTram)
    {
        return maxHP* phanTram / 100;
    }
    private void ApplyHPNumberATK()
    {
        if (hpEnemy > PhanTramHP(75)) //attackNumber = 0 , 1
        {
            attackNumber = RandomATK(enemy.phanTramList[0].atk0, enemy.phanTramList[0].atk1, enemy.phanTramList[0].atk2, enemy.phanTramList[0].atk3, enemy.phanTramList[0].atk4);
        }
        else if (hpEnemy > PhanTramHP(50) && hpEnemy < PhanTramHP(75))
        {
            attackNumber = RandomATK(enemy.phanTramList[1].atk0, enemy.phanTramList[1].atk1, enemy.phanTramList[1].atk2, enemy.phanTramList[1].atk3, enemy.phanTramList[1].atk4);
        }
        else if (hpEnemy > PhanTramHP(30) && hpEnemy < PhanTramHP(50))
        {
            attackNumber = RandomATK(enemy.phanTramList[2].atk0, enemy.phanTramList[2].atk1, enemy.phanTramList[2].atk2, enemy.phanTramList[2].atk3, enemy.phanTramList[2].atk4);
        }
        else if (hpEnemy < PhanTramHP(30))
        {
            attackNumber = RandomATK(enemy.phanTramList[3].atk0, enemy.phanTramList[3].atk1, enemy.phanTramList[3].atk2, enemy.phanTramList[3].atk3, enemy.phanTramList[3].atk4);
        }
        //if (attackNumber < 0 || attackNumber >= 5)
        //{
        //    attackNumber = 0;
        //}
    }

    int RandomATK(int percent0, int percent1, int percent2, int percent3, int percent4)
    {
        // Xác định tỷ lệ phần trăm cho mỗi số
        int[] percentages = { percent0, percent1, percent2, percent3, percent4 };

        // Tính tổng tỷ lệ phần trăm (nên là 100)
        int totalPercentage = 0;
        foreach (int percentage in percentages)
        {
            totalPercentage += percentage;
        }

        // Random một số từ 0 đến tổng tỷ lệ phần trăm
        int randomPoint = Random.Range(0, totalPercentage);

        // Xác định số dựa trên điểm random
        int accumulatedPercentage = 0;
        for (int i = 0; i < percentages.Length; i++)
        {
            accumulatedPercentage += percentages[i];
            if (randomPoint < accumulatedPercentage)
            {
                return i;
            }
        }

        // Trường hợp mặc định, nếu có lỗi (không xảy ra)
        return -1;
    }
}
