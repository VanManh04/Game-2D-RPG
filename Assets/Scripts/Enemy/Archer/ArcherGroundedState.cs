using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Enemy_Archer enemy;
    protected Transform player;

    public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2f)
            stateMachine.ChangeState(enemy.battleState);
    }
}
