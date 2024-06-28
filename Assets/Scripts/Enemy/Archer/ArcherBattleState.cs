using UnityEngine;

public class ArcherBattleState : EnemyState
{

    private Transform player;
    private Enemy_Archer enemy;
    private int moveDir;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead && player.GetComponent<PlayerStats>() != null)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.safeDistance)
            {
                if (CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
                //else
                //    stateMachine.ChangeState(enemy.attackTAY); tan cong bang tay
            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                //Debug.Log("I Attack");
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7f)
                stateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipController();

        //Can move -> player
        //if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
        //    return;

        //enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private void BattleStateFlipController()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
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

    private bool CanJump()
    {
        if (enemy.GroundBehind() == false || enemy.WallBehind() == true)
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {

            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }
}