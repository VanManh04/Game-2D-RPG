using UnityEngine;

public class ArcherAttackState : EnemyState
{

    private Enemy_Archer enemy;
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _startMachine, _animBoolName)
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