using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;
    public int health = 2;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRadius = 5f;
    public float stopDistance = 0.5f;

    private Vector2 startPosition;
    private float groundY;
    private SpriteRenderer sr;
    private Animator anim;

    private bool isDead = false;
    private bool isKnocked = false;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.15f;

    AudioManager audioManager;

    private Rigidbody2D rb;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damage, Vector2 attackerPos)
    {
        if (isKnocked) return;      // cegah knockback dobel-dobel

        health -= damage;
        Debug.Log("Enemy kena hit! Sisa HP: " + health);

        audioManager.PlaySFX(audioManager.Slime);

        anim.SetTrigger("IsHurt");    // animasi hurt

        // mulai knockback
        StartCoroutine(Knockback(attackerPos));

        if (health <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator Knockback(Vector2 attackerPos)
    {
        isKnocked = true;

        // arah knockback: menjauh dari player
        float direction = transform.position.x - attackerPos.x;
        direction = Mathf.Sign(direction);

        rb.velocity = new Vector2(direction * knockbackForce, rb.velocity.y);

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = new Vector2(0, rb.velocity.y);
        isKnocked = false;  // kembali normal
    }

    void Die()
    {
        isDead = true;
        isKnocked = true;          // supaya AI benar-benar berhenti

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsIdle", false);

        anim.SetTrigger("IsDead");

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;     // NEW → hentikan physics

        // NONAKTIFKAN COLLIDER → biar tidak tabrakan setelah mati
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("Enemy mati");

        Destroy(gameObject, 2f);   // delay sebelum musuh hilang
    }

    void Start()
    {
        startPosition = transform.position;
        groundY = transform.position.y;

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;
        if (isDead) return;
        if (isKnocked) return;  // ===== SAAT HURT, AI TIDAK GERAK =====

        float fullDistance = Vector2.Distance(transform.position, player.position);
        float horizontalDist = Mathf.Abs(player.position.x - transform.position.x);

        Vector2 currentPos = transform.position;

        // =======================
        // CHASE PLAYER
        // =======================
        if (fullDistance <= chaseRadius)
        {
            if (horizontalDist > stopDistance)
            {
                float newX = Mathf.MoveTowards(
                    currentPos.x,
                    player.position.x,
                    moveSpeed * Time.deltaTime
                );

                transform.position = new Vector2(newX, groundY);

                anim.SetBool("IsWalking", true);
                anim.SetBool("IsIdle", false);
            }
            else
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsIdle", true);
            }

            FlipSprite(player.position.x - transform.position.x);
        }
        else
        {
            // =======================
            // KEMBALI KE POSISI AWAL
            // =======================
            float backDistance = Mathf.Abs(transform.position.x - startPosition.x);

            if (backDistance > 0.1f)
            {
                float newX = Mathf.MoveTowards(
                    currentPos.x,
                    startPosition.x,
                    moveSpeed * Time.deltaTime
                );

                transform.position = new Vector2(newX, groundY);

                anim.SetBool("IsWalking", true);
                anim.SetBool("IsIdle", false);

                FlipSprite(startPosition.x - transform.position.x);
            }
            else
            {
                transform.position = new Vector2(transform.position.x, groundY);

                anim.SetBool("IsWalking", false);
                anim.SetBool("IsIdle", true);
            }
        }
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0)
            sr.flipX = false;
        else if (direction < 0)
            sr.flipX = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
