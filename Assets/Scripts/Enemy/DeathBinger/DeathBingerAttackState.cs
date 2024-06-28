using System.Collections;
using UnityEngine;

public class DeathBingerAttackState : EnemyState
{

    private Enemy_DeathBinger enemy;
    public DeathBingerAttackState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_DeathBinger _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            if (enemy.CanTeleport())
                stateMachine.ChangeState(enemy.teleportState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}