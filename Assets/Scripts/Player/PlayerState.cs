using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// PlayerState la lop co so cho trang thai cua nguoi choi
public class PlayerState
{
    protected PlayerStateMachine stateMachine;  // Tham chieu toi may trang thai cua nguoi choi
    protected Player player;  // Tham tham chieu toi nguoi choi

    protected Rigidbody2D rb;  // Tham chieu toi Rigidbody2D cua nguoi cho

    protected float xInput;  // Bien luu gia tri cua truc X
    protected float yInput;  // Bien luu gia tri cua truc Y
    protected string animBoolName;  // Ten bien bool trong Animator de dieu khỉn hoat anh

    protected float stateTimer;  // bo dem thoi gian cho trang thai
    protected bool triggerCalled;  // Co de kiem tra xem trigger da duoc goi hay chue


    //private InputController inputController;
    //private InputAction move;

    // Ham dung cua PlayerState
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;  // Gan tham chieu toi doi tuong nguoi choi
        this.stateMachine = _stateMachine;  // Gan tham chieu toi may trang thai
        this.animBoolName = _animBoolName;  // Gan ten bien bool trong Animation
    }

    // Enter duoc goi khi trang thai nay duoc kich hoat
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true); // Bat hoat anh tham chieu
        rb = player.rb; // Luu tham chieu toi RB
        triggerCalled = false;  // Dat lai co Trigger
    }

    // Goi moi frame
    public virtual void Update()
    {
        //Debug.Log("I am in " + animBoolName);

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);

        //inputController = new InputController();
        //move = inputController.Player.Move;
        //rb.velocity = move.ReadValue<Vector2>() * 10 * player.moveSpeed;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);    // Tat hoat anh tuong ung
    }

    // Phuong thuc AnimationFinishTrigger duoc goi khi hoat anh ket thuc
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;   // Dat co trigger thanh true
    }
}
