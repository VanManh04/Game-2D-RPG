using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();
    private Transform player => PlayerManager.instance.player.transform;

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target);

                // hit.GetComponent<Player>().Damage();
            }
        }
    }

    public void AttackTrigger30PhanTram()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamagePhanTram(_target,30);
            }
        }
    }

    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    public void OpenCounterWindow() => enemy.OpenCounterAttackWindow();

    public void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

    public void FlipToFacePlayer()
    {
        if(player.position.x>enemy.transform.position.x&&enemy.facingDir==-1)
            enemy.Flip();
        else if(player.position.x<enemy.transform.position.x&&enemy.facingDir==1)
            enemy.Flip();
    }

    public void FireKnightBullet()
    {
        Enemy_FireKnight fk = GetComponentInParent<Enemy_FireKnight>();
        fk?.SpawnFireKnight();
    }

    public void FireKnightBulletBIG()
    {
        Enemy_FireKnight fk = GetComponentInParent<Enemy_FireKnight>();
        fk?.SpawnFireKnightBIG();
    }
}
