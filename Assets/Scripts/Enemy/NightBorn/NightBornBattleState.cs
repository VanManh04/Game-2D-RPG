using UnityEngine;

public class NightBornBattleState : EnemyState
{

    private Enemy_NightBorn enemy;
    private Transform player;
    private int moveDir;

    private bool flippedOnce;
    public NightBornBattleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_NightBorn _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead && player.GetComponent<PlayerStats>() != null)
            stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
        flippedOnce = false;
    }

    public override void Update()
    {
        base.Update();
        enemy.anim.SetFloat("xVelocity", enemy.rb.velocity.x);

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                //Debug.Log("I Attack");
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7f)
                stateMachine.ChangeState(enemy.idleState);
        }


        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if (distanceToPlayerX < .8f)
            return;

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);

            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}