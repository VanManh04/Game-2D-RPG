using UnityEngine;


public class FireKnightBattleState : EnemyState
{
    private Enemy_FireKnight enemy;
    private Player player;
    private int moveDir;

    private bool flippedOnce;

    public FireKnightBattleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;

        if (player.GetComponent<PlayerStats>().isDead && player.GetComponent<PlayerStats>() != null)
            stateMachine.ChangeState(enemy.idleState);

        stateTimer = enemy.battleTime;
        flippedOnce = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.anim.SetFloat("xVelocity", enemy.rb.velocity.x);


        if (Mathf.Abs(player.transform.position.x - enemy.transform.position.x) < .4f)
            stateMachine.ChangeState(enemy.attackState);

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


        //float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        //if (distanceToPlayerX < .8f)
        //    return;

        if (player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
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
