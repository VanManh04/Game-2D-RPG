using System.Collections;
using UnityEngine;

public class DeathBingerIdleState : EnemyState
{
    private Enemy_DeathBinger enemy;
    private Transform player;
    public DeathBingerIdleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_DeathBinger _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if(Vector2.Distance(player.position, enemy.transform.position) < 7f)
            enemy.bossFightBegun = true;

        if (stateTimer < 0 && enemy.bossFightBegun) 
            stateMachine.ChangeState(enemy.battleState);
    }
}