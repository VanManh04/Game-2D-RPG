public class FireKnightMoveState : FireKnightGroundedState
{
    public FireKnightMoveState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName, _enemy)
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