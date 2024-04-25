using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected bool triggerCalled;
    protected string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemy, EnemyStateMachine _startMachine, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _startMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName,true);
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName,false);
    }
}
