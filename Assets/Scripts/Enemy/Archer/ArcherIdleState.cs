﻿public class ArcherIdleState : ArcherGroundedState
{
    public ArcherIdleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _startMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}