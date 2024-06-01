using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;  //huong knockback
    [SerializeField] protected float knockbackDuration;     //thoi gian knock back
    protected bool isKnockback;     //Co de kiem tra knockback

    [Header("Collision info")]
    public Transform attackCheck;       // Tham chieu toi Transform de kiem tra attack
    public float attackCheckRadius;     //ban kinh kiem tra Attack
    [SerializeField] protected Transform groundCheck;       //Tham chieu toi Tranform de kiem tra mat fat
    [SerializeField] protected float groundCheckDistance;       //Khoang cach de kiem tra mat dat
    [SerializeField] protected Transform wallCheck;       //Tham chieu toi Tranform de kiem tra tuong
    [SerializeField] protected float wallCheckDistance;       //Khoang cach de kiem tra tuong
    [SerializeField] protected LayerMask whatIsGround;       //Layer kiem tra mat dat


    public int facingDir { get; private set; } = 1;       //Huong doi mat
    protected bool facingRight = true;       //co de kiem tra huong doi mat

    public System.Action onFlipped;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    // Ph??ng th?c n�y ???c g?i khi Entity b? t?n c�ng
    // K�ch ho?t hi?u ?ng flash v� knockback
    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
        //Debug.Log(gameObject.name + " was damaged!");
    }
    
    protected virtual IEnumerator HitKnockback()// Ph??ng th?c n�y x? l� knockback khi Entity b? t?n c�ng
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDirection.x*-facingDir, knockbackDirection.y);
        
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();

        if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnockback)
            return;

        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockback)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    // Ph??ng th?c n�y ??t m�u s?c c?a Entity
    public void Maketransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    public virtual void Die()
    {

    }
}
