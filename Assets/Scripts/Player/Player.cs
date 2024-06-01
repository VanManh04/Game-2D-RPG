using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;// C�c h??ng t?n c�ng c?a ng??i ch?i
    public float counterAttackDuration = .2f;// Th?i gian ph?n c�ng

    public bool isBusy { get; private set; }// Tr?ng th�i b?n c?a ng??i ch?i

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;// ?nh h??ng khi ki?m tr? v?

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }


    public SkillManager skill {  get; private set; }// Tr�nh qu?n l� k? n?ng
    public GameObject sword; // { get; private set; }   // Ki?m c?a ng??i ch?i

    #region State
    public PlayerStateMachine stateMachine { get; private set; }// M�y tr?ng th�i c?a ng??i ch?i
    
    // C�c tr?ng th�i c?a ng??i ch?i
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
        stateMachine = new PlayerStateMachine();// Kh?i t?o m�y tr?ng th�i

        // Kh?i t?o c�c tr?ng th�i
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

        skill = SkillManager.instance;// G�n tr�nh qu?n l� k? n?ng

        stateMachine.Initialize(idleState);// Kh?i t?o m�y tr?ng th�i v?i tr?ng th�i ban ??u l� idle
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();// G?i ph??ng th?c Update c?a tr?ng th�i hi?n t?i

        CheckForDashInput();// Ki?m tra ??u v�o ?? th?c hi?n dash

        if (Input.GetKeyDown(KeyCode.F))// Ki?m tra v� s? d?ng k? n?ng n?u ph�m F ???c nh?n
            skill.crystal.CanUseSkill();
    }

    //G�n m?t ??i t??ng ki?m m?i cho ng??i ch?i
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;// G�n ki?m m?i cho ng??i ch?i
    }

    // Chuy?n sang tr?ng th�i b?t ki?m v� h?y ??i t??ng ki?m
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);// Chuy?n sang tr?ng th�i b?t ki?m
        Destroy(sword);
    }

    //public void ExitBlackHoleAbility()
    //{
    //    stateMachine.ChangeState(airState);
    //}


    //??t tr?ng th�i b?n trong m?t kho?ng th?i gian nh?t ??nh
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //G?i trigger k?t th�c ho?t ?nh c?a tr?ng th�i hi?n t?i
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    //Ki?m tra ??u v�o ?? th?c hi?n dash, n?u kh�ng ph�t hi?n t??ng v� ph�m dash ???c nh?n.
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

    //G?i ph??ng th?c ch?t c?a l?p c? s? v� chuy?n sang tr?ng th�i ch?t khi ng??i ch?i ch?t.
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
