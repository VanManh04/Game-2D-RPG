using UnityEngine;


public class NightBornDeadState : EnemyState
{

    private Enemy_NightBorn enemy;
    public NightBornDeadState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_NightBorn _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        //stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            enemy.SelfDestroy();
    }
}