using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBingerBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBinger enemy;
    private int moveDir;

    public DeathBingerBattleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_DeathBinger _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        //if (player.GetComponent<PlayerStats>().isDead && player.GetComponent<PlayerStats>() != null)
        //    stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                //Debug.Log("I Attack");
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else 
                    stateMachine.ChangeState(enemy.idleState);
            }
        }


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
