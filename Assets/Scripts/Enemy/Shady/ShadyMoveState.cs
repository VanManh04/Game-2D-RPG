﻿using System.Collections;
using UnityEngine;

public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _startMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}