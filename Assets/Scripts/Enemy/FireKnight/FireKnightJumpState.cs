using UnityEngine;

public class FireKnightJumpState : EnemyState
{
    private Enemy_FireKnight enemy;
    private Player player;
    private float flyTime = .4f;

    private bool skillUsed;
    public FireKnightJumpState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_FireKnight _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;

        stateTimer = flyTime;
        enemy.rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (stateTimer > 0)
        //    rb.velocity = new Vector2(0, player.transform.position.y);
        rb.velocity = new Vector2(0, 20f);

        //if (stateTimer < 0)
        //{
        //    rb.velocity = new Vector2(0, -.1f);
        //    stateMachine.ChangeState(enemy.attackJumpState);
        //}
        if ((enemy.transform.position.y + 1.5f) > player.transform.position.y)
        {
            rb.velocity = new Vector2(0, -.1f);
            stateMachine.ChangeState(enemy.attackJumpState);
        }
    }
}
