using UnityEngine;

public class FireKnight_controller : MonoBehaviour
{
    [Header("Knight")]
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask WhatIsPlayer;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int damageKnight;
    [SerializeField] private Enemy_FireKnight enemy;

    private CharacterStats myStats;

    [Header("Fire")]
    [SerializeField] private bool FireKnightBig_controller;
    [SerializeField] private float speedFire;
    [SerializeField] private float radiusFire;
    [SerializeField] private int damageFire;

    public void SetupKnight(CharacterStats _stats, bool _FireKnightBig_controller)
    {
        enemy = _stats.GetComponent<Enemy_FireKnight>();
        myStats = _stats;
        FireKnightBig_controller = _FireKnightBig_controller;
    }

    private void Start()
    {
        if (enemy.facingDir == -1)
        {
            speed = -speed;
            speedFire = -speedFire;
            transform.Rotate(0, 180, 0);
        }

    }

    void Update()
    {
        if (!FireKnightBig_controller)
        {
            rb.velocity = new Vector3(speed, 0, 0);

            Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, WhatIsPlayer);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    //Debug.Log("Player detected!");

                    // Optional: thực hiện các hành động khác nếu cần
                    hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                    hit.GetComponent<CharacterStats>()?.TakeDamage(damageKnight);
                    //myStats.DoDamage(hit.GetComponent<CharacterStats>());
                    //Debug.Log("Player damage");

                    // Optional: Uncomment nếu bạn muốn sử dụng ScreenShake
                    hit.GetComponent<Player>()?.fx.ScreenShake(new Vector3(2, 2));

                    Destroy(this.gameObject);
                }
            }
        }else
        {
            rb.velocity = new Vector3(speedFire, 0, 0);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, radiusFire, WhatIsPlayer);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    //Debug.Log("Player detected!");

                    // Optional: thực hiện các hành động khác nếu cần
                    hit.GetComponent<CharacterStats>()?.TakeDamage(damageFire);
                    //hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                    myStats.DoDamage(hit.GetComponent<CharacterStats>());
                    //Debug.Log("Player damage");

                    // Optional: Uncomment nếu bạn muốn sử dụng ScreenShake
                    hit.GetComponent<Player>()?.fx.ScreenShake(new Vector3(2, 3));

                    Destroy(this.gameObject);
                }
            }
        }


    }

    private void OnDrawGizmos()
    {
        if (!FireKnightBig_controller)
            Gizmos.DrawWireCube(check.position, boxSize);
        else
            Gizmos.DrawWireSphere(check.position, radiusFire);
    }
}
