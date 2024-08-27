using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhanTram
{
    public int atk0, atk1, atk2, atk3, atk4;
}

public class Enemy_FireKnight : Enemy
{
    public float defaultGravity;
    public SpriteRenderer sr;
    [Header("Attack Check")]
    public Vector2[] offSetPlus;
    public float[] offSetRadius;

    [Header("Spawn Knight")]
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject knightBIGPrefab;
    [SerializeField] private Transform posSpawn;

    [Header("Check Defend")]
    [SerializeField] private Transform checkDefend;
    [SerializeField] private Vector2 sizeBox;
    [SerializeField] private LayerMask WhatIsSword;

    [Header("Jump Attack")]
    [SerializeField] private Transform checkJumpAttack;
    [SerializeField] private Vector2 sizeBoxJumpAttack;

    [Header("Phan tram AttackNumber")]
    public List<PhanTram> phanTramList = new List<PhanTram>();
    #region

    public FireKnightIdleState idleState { get; private set; }
    public FireKnightMoveState moveState { get; private set; }
    public FireKnightBattleState battleState { get; private set; }
    public FireKnightJumpState jumpState { get; private set; }
    public FireKnightDefendState defendState { get; private set; }
    public FireKnightAttackState attackState { get; private set; }
    public FireKnightAttackJumpState attackJumpState { get; private set; }
    public FireKnightStunnedState stunnedState { get; private set; }
    public FireKnightDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new FireKnightIdleState(this, stateMachine, "Idle", this);
        moveState = new FireKnightMoveState(this, stateMachine, "Move", this);
        battleState = new FireKnightBattleState(this, stateMachine, "Battle", this);
        jumpState = new FireKnightJumpState(this, stateMachine, "Jump", this);
        defendState = new FireKnightDefendState(this, stateMachine, "Defend", this);
        attackState = new FireKnightAttackState(this, stateMachine, "Attack", this);
        attackJumpState = new FireKnightAttackJumpState(this, stateMachine, "atkAir", this);
        stunnedState = new FireKnightStunnedState(this, stateMachine, "Stunned", this);
        deadState = new FireKnightDeadState(this, stateMachine, "Deadth", this);
    }

    protected override void Start()
    {
        base.Start();

        defaultGravity = rb.gravityScale;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        CheckDefend();
        CheckJumpAttack();
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

    public void SpawnFireKnight()
    {
        GameObject knight = Instantiate(knightPrefab, posSpawn.position, Quaternion.identity);
        knight.GetComponent<FireKnight_controller>().SetupKnight(stats, false);
    }

    public void SpawnFireKnightBIG()
    {
        GameObject knight = Instantiate(knightBIGPrefab, posSpawn.position, Quaternion.identity);
        knight.GetComponent<FireKnight_controller>().SetupKnight(stats, true);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void SelfDestroy() => Destroy(gameObject);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(checkDefend.position, sizeBox);
        Gizmos.DrawWireCube(checkJumpAttack.position, sizeBoxJumpAttack);
    }

    private void CheckDefend()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkDefend.position, sizeBox, WhatIsSword);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Sword_Skill_Controller>() != null)
            {
                stateMachine.ChangeState(defendState);
            }
        }
    }

    private void CheckJumpAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkJumpAttack.position, sizeBoxJumpAttack, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //Debug.Log("JUMP Attack");
                //Debug.Log(hit.GetComponent<Transform>().position.y - transform.position.y);
                if ((hit.GetComponent<Transform>().position.y - transform.position.y) >= 3)
                    stateMachine.ChangeState(jumpState);
            }
        }
    }
}
