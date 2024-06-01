using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;      // Tham chiếu đến thành phần Animator của đối tượng kiếm, cho phép điều khiển các hoạt ảnh.
    private Rigidbody2D rb;      // Tham chiếu đến thành phần Rigidbody2D của đối tượng kiếm, cho phép xử lý vật lý.
    private CircleCollider2D cd;      // Tham chiếu đến thành phần CircleCollider2D của đối tượng kiếm, dùng để xử lý va chạm.
    private Player player;      // Tham chiếu đến người chơi, giúp trong việc trả kiếm về cho người chơi khi cần thiết.

    private bool canRotate = true;      // Biến kiểm soát xem kiếm có thể quay được không.
    private bool isReturning;      // Biến kiểm soát xem kiếm đang trở về hay không.

    private float freezeTimeDuration;      // Thời gian đóng băng khi kiếm gây sát thương cho mục tiêu.
    private float returnSpeed = 12f;      // Tốc độ trở về của kiếm khi nó được gọi trở lại.

    [Header("Pierce info")]//Kiếm xuyên mục tiêu
    [SerializeField] private float pierceAmount;      //Số lần kiếm có thể xuyên qua mục tiêu.

    [Header("Bounce info")]//Kiếm quay từ mục tiêu này sang mục tiêu khác rồi trở về
    private float bounceSpeed;      // Tốc độ khi kiếm nảy qua các mục tiêu.
    private bool isBouncing;      // Biến kiểm soát xem kiếm có đang nảy không.
    private int bounceAmount;      // Số lần kiếm có thể nảy qua các mục tiêu.
    private List<Transform> enemyTarget;      // Danh sách mục tiêu của kiếm khi nó đang nảy.
    private int targetIndex;      // Chỉ số của mục tiêu hiện tại mà kiếm đang nhắm vào khi nó đang nảy.

    [Header("Spin info")]//Kiếm quay 1 đường thẳng mình ném xong trở về
    private float maxTravelDistance;      // Khoảng cách tối đa mà kiếm có thể di chuyển trước khi dừng quay.
    private float spinDuration;      // Thời gian mà kiếm sẽ quay trước khi quay trở lại.
    private float spinTimer;      // Đếm thời gian cho tính năng quay của kiếm.
    private bool wasStopped;      // Biến kiểm soát xem kiếm đã dừng lại khi quay hay chưa.
    private bool isSpinning;      // Biến kiểm soát xem kiếm đang quay hay không.

    private float hitTimer;      // Đếm thời gian giữa các lần gây sát thương khi kiếm đang quay.
    private float hitCooldown;      // Thời gian giữa các lần gây sát thương khi kiếm đang quay.

    private float spinDirection;      // Hướng quay của kiếm.

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    //Cấu hình thông số cho kiếm như hướng, trọng lực, thời gian đóng băng, và tốc độ trở về.
    public void SetupSword(Vector2 _dir, float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if(pierceAmount<=0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);
    }

    //Cấu hình thông số cho tính năng nảy, bao gồm số lần nảy và tốc độ nảy.
    public void SetupBounce(bool _isBouncing,int _amountOfBounces,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    //Cấu hình thông số cho tính năng xuyên qua.
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    // Cấu hình thông số cho tính năng quay, bao gồm khoảng cách tối đa, thời gian quay, và thời gian nghỉ giữa các lần quay.
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration,float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    //Trả lại kiếm về cho người chơi.
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    //Xử lý logic cho tính năng quay của kiếm.
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    // Dừng kiếm khi đang quay.
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    //Xử lý logic cho tính năng nảy của kiếm.
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    //Xử lý khi kiếm va chạm vào các đối tượng, bao gồm tính năng gây sát thương, xuyên qua, nảy, và dính vào.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsGorBounce(collision);

        StuckInto(collision);
    }

    //Gây sát thương cho mục tiêu và bắt đầu thời gian đóng băng.
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    //Thiết lập mục tiêu cho tính năng nảy.
    private void SetupTargetsGorBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    //Xử lý khi kiếm đâm vào đối tượng và dính vào hoặc dừng lại.
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count>0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
