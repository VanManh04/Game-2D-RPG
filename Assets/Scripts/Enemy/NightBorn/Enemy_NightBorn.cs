public class Enemy_NightBorn : Enemy
{
    #region States

    public NightBornIdleState idleState { get; private set; }
    public NightBornMoveState moveState { get; private set; }
    public NightBornBattleState battleState { get; private set; }
    public NightBornAttackState attackState { get; private set; }
    public NightBornStunnedState stunnedState { get; private set; }
    public NightBornDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new NightBornIdleState(this, stateMachine, "Idle", this);
        moveState = new NightBornMoveState(this, stateMachine, "Move", this);
        battleState = new NightBornBattleState(this, stateMachine, "Battle", this);
        attackState = new NightBornAttackState(this, stateMachine, "Attack", this);
        stunnedState = new NightBornStunnedState(this, stateMachine, "Stunned", this);
        deadState = new NightBornDeadState(this, stateMachine, "Deadth", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void SelfDestroy() => Destroy(gameObject);
}
