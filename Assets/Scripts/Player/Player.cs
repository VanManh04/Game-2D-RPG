using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;// Các h??ng t?n công c?a ng??i ch?i
    public float counterAttackDuration = .2f;// Th?i gian ph?n công

    public bool isBusy { get; private set; }// Tr?ng thái b?n c?a ng??i ch?i

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;// ?nh h??ng khi ki?m tr? v?

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }


    public SkillManager skill {  get; private set; }// Trình qu?n lý k? n?ng
    public GameObject sword; // { get; private set; }   // Ki?m c?a ng??i ch?i

    #region State
    public PlayerStateMachine stateMachine { get; private set; }// Máy tr?ng thái c?a ng??i ch?i
    
    // Các tr?ng thái c?a ng??i ch?i
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
        stateMachine = new PlayerStateMachine();// Kh?i t?o máy tr?ng thái

        // Kh?i t?o các tr?ng thái
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

        skill = SkillManager.instance;// Gán trình qu?n lý k? n?ng

        stateMachine.Initialize(idleState);// Kh?i t?o máy tr?ng thái v?i tr?ng thái ban ??u là idle
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();// G?i ph??ng th?c Update c?a tr?ng thái hi?n t?i

        CheckForDashInput();// Ki?m tra ??u vào ?? th?c hi?n dash

        if (Input.GetKeyDown(KeyCode.F))// Ki?m tra và s? d?ng k? n?ng n?u phím F ???c nh?n
            skill.crystal.CanUseSkill();
    }

    //Gán m?t ??i t??ng ki?m m?i cho ng??i ch?i
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;// Gán ki?m m?i cho ng??i ch?i
    }

    // Chuy?n sang tr?ng thái b?t ki?m và h?y ??i t??ng ki?m
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);// Chuy?n sang tr?ng thái b?t ki?m
        Destroy(sword);
    }

    //public void ExitBlackHoleAbility()
    //{
    //    stateMachine.ChangeState(airState);
    //}


    //??t tr?ng thái b?n trong m?t kho?ng th?i gian nh?t ??nh
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //G?i trigger k?t thúc ho?t ?nh c?a tr?ng thái hi?n t?i
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    //Ki?m tra ??u vào ?? th?c hi?n dash, n?u không phát hi?n t??ng và phím dash ???c nh?n.
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    //G?i ph??ng th?c ch?t c?a l?p c? s? và chuy?n sang tr?ng thái ch?t khi ng??i ch?i ch?t.
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
