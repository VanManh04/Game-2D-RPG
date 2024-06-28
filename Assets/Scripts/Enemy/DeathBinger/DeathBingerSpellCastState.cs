using UnityEngine;

public class DeathBingerSpellCastState : EnemyState
{
    private Enemy_DeathBinger enemy;

    private int amountOfSpells;
    private float spellTimer;

    public DeathBingerSpellCastState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_DeathBinger _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells = enemy.amountOfSells;
        stateTimer = .5f;
    }

    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();//Cast as a spell
        
        if(amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}