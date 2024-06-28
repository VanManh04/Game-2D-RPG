using System.Collections;
using UnityEngine;

public class DeathBingerDeadState : EnemyState
{
    private Enemy_DeathBinger enemy;
    public DeathBingerDeadState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_DeathBinger _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}