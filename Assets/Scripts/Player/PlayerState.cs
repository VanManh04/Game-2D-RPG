using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState l� l?p c? s? cho c�c tr?ng th�i c?a ng??i ch?i
public class PlayerState
{
    protected PlayerStateMachine stateMachine;  // Tham chi?u t?i m�y tr?ng th�i c?a ng??i ch?i
    protected Player player;  // Tham chi?u t?i ??i t??ng ng??i ch?i

    protected Rigidbody2D rb;  // Tham chi?u t?i Rigidbody2D c?a ng??i ch?i

    protected float xInput;  // Bi?n l?u tr? gi� tr? nh?p tr?c X
    protected float yInput;  // Bi?n l?u tr? gi� tr? nh?p tr?c 
    protected string animBoolName;  // T�n bi?n bool trong Animator ?? ?i?u khi?n ho?t ?nh

    protected float stateTimer;  // B? ??m th?i gian cho tr?ng th�i
    protected bool triggerCalled;  // C? ?? ki?m tra xem trigger ?� ???c g?i hay ch?a


    // H�m d?ng c?a PlayerState
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;  //G�n tham chi?u t?i ??i t??ng ng??i ch?i
        this.stateMachine = _stateMachine;  //G�n tham chi?u t?i m�y tr?ng th�i
        this.animBoolName = _animBoolName;  //G�n t�n bi?n bool trong Animation
    }

    //Enter ???c g?i khi tr?ng th�i n�y ???c k�ch ho?t
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true); //B?t ho?t ?nh tham chi?u
        rb = player.rb; //L?u tham chi?u t?i RB
        triggerCalled = false;  //??t l?i c? Trigger
    }

    //G?i m?i frame
    public virtual void Update()
    {
        //Debug.Log("I am in " + animBoolName);

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);    //T?t ho?t ?nh t??ng ?ng
    }

    // Ph??ng th?c AnimationFinishTrigger ???c g?i khi ho?t ?nh k?t th�c
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;   // ??t c? trigger th�nh true
    }
}
