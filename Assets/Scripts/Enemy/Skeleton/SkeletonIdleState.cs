using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    private Enemy_Skeleton enemy;
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName, _enemy)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(14,enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
