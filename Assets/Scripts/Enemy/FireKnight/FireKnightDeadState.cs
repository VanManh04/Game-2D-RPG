using System.Collections;
using UnityEngine;

public class FireKnightDeadState : EnemyState
{
    private Enemy_FireKnight enemy;
    public FireKnightDeadState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        enemy = _enemy;
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
        if (triggerCalled)
        {
            enemy.sr.color = new Color(1, 1, 1, enemy.sr.color.a - (Time.deltaTime * .5f));
        }
    }
}
