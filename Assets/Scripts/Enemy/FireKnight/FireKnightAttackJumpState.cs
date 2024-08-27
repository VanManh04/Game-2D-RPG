using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnightAttackJumpState : EnemyState
{
    private Enemy_FireKnight enemy;
    private Player player;

    public FireKnightAttackJumpState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.rb.gravityScale = enemy.defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
