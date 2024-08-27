using System.Collections;
using UnityEngine;

public class NightBornAttackState : EnemyState
{
    private Enemy_NightBorn enemy;
    public NightBornAttackState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_NightBorn _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}