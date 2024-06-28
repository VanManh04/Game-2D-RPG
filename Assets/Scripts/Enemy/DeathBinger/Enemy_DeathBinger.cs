using UnityEngine;

public class Enemy_DeathBinger : Enemy
{
    public bool bossFightBegun;


    [Header("Spell cast detals")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;

    [Header("Teleport derails")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 sourroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;


    #region States

    public DeathBingerBattleState battleState { get; private set; }
    public DeathBingerAttackState attackState { get; private set; }
    public DeathBingerIdleState idleState { get; private set; }
    public DeathBingerDeadState deadState { get; private set; }
    public DeathBingerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);
        idleState = new DeathBingerIdleState(this, stateMachine, "Idle", this);

        battleState = new DeathBingerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBingerAttackState(this, stateMachine, "Attack", this);

        deadState = new DeathBingerDeadState(this, stateMachine, "Idle", this);
        spellCastState = new DeathBingerSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;
        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * spellOffset.x;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsArownd())
        {
            Debug.Log("Looking for new position");
            FindPosition();
        }

    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);

    private bool SomethingIsArownd() => Physics2D.BoxCast(transform.position, sourroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, sourroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast()
    {

        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }

        return false;
    }
}
