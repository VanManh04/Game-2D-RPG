using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;// cac huong tan cong cua nguoi cho
    public float counterAttackDuration = .2f;// thoi gian phan cong

    public bool isBusy { get; private set; }// trang thai ban cua nguoi choi

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;// anh huong khi kiem tro ve
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }


    public SkillManager skill {  get; private set; }// trinh quan ly skill
    public GameObject sword; // { get; private set; }   // kiem cua nguoi choi
    public PlayerFX fx {  get; private set; }

    #region State
    public PlayerStateMachine stateMachine { get; private set; }// may trang thai cua nguoi choi

    // Các trang thai cua nguoi choi
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; } 
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();// khoi tao trang thai

        // khoi tao trang thai
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        fx=GetComponent<PlayerFX>();

        skill = SkillManager.instance;// gan trinh quan ly skill

        stateMachine.Initialize(idleState);// khoi tao may trang thai voi trang thai ban dau la idle

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();
        stateMachine.currentState.Update();// goi phuong thuc update cua trang thai hien tai

        CheckForDashInput();// kiem tra dau vao de thuc hien Dash

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked) 
            skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))//Debug.Log("use flash");
            Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDutation)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDutation);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    //gan mot doi tuong kiem cho nguoi choi
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    // chuyen sang trang thai bat lay kiem va huy doi tuong kiem
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    //public void ExitBlackHoleAbility()
    //{
    //    stateMachine.ChangeState(airState);
    //}


    //bat trang thai ban trong khoang thoi fian nhat dinh
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //goi trigger va ket thuc hoat anh trang thai hien tai
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if(skill.dash.dashUnlocked == false)
                    return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
    
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
