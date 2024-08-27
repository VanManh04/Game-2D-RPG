using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7, 12);  //huong knockback
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(.5f, 2);
    [SerializeField] protected float knockbackDuration = .07f;     //thoi gian knock back
    protected bool isKnockback;     //Co de kiem tra knockback

    [Header("Collision info")]
    public Transform attackCheck;       // Tham chieu toi Transform de kiem tra attack
    public float attackCheckRadius = 1.2f;     //ban kinh kiem tra Attack
    [SerializeField] protected Transform groundCheck;       //Tham chieu toi Tranform de kiem tra mat fat
    [SerializeField] protected float groundCheckDistance = 1;       //Khoang cach de kiem tra mat dat
    [SerializeField] protected Transform wallCheck;       //Tham chieu toi Tranform de kiem tra tuong
    [SerializeField] protected float wallCheckDistance = .8f;       //Khoang cach de kiem tra tuong
    [SerializeField] protected LayerMask whatIsGround;       //Layer kiem tra mat dat

    public int knockbackDir { get; private set; }

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
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDutation)//lam cham Entity bang phan tram lam cham va thoi gian lam cham
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    //goi khi entity bo tan cong va bat FX...
    public virtual void DamageImpact() => StartCoroutine("HitKnockback");
    //Debug.Log(gameObject.name + " was damaged!");

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;
    }

    public void SetupKnockbackPower(Vector2 _knockbackpower) => knockbackPower = _knockbackpower;

    protected virtual IEnumerator HitKnockback()// xu ly knockback khi entity bi tan cong
    {
        isKnockback = true;

        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);
        //if (knockbackPower.x > 0 || knockbackPower.y > 0)
        rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;

        SetupZeroKnockbackPower();
    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
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

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
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

    public virtual void Die()
    {

    }

    public void SetupAttackCheck(Vector2 _tranform, float _radius)
    {
        attackCheck.transform.localPosition = new Vector3(_tranform.x, _tranform.y, 0);
        attackCheckRadius = _radius;
    }
}
