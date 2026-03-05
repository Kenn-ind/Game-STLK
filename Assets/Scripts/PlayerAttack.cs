using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 0.5f;
    public int damage = 1;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Player movement;
    Animator anim;

    public float attackCooldown = 0.3f;
    private float nextAttackTime = 0f;

    AudioManager audioManager;


    public Vector2 attackPointRight = new Vector2(0.2f, 0);
    public Vector2 attackPointLeft = new Vector2(-0.2f, 0);

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        attackPoint.localPosition = new Vector3(0.2f, 0f, 0f);
    }

    void Update()
    {

        bool attackInput = Input.GetKeyDown(KeyCode.E) || MobileInput.attack;

        if (!movement.isGrounded)
        {
            MobileInput.attack = false;
            return;
        }

        if (attackInput && Time.time < nextAttackTime)
        {
            MobileInput.attack = false;
        }

        if (attackInput && Time.time >= nextAttackTime)
        {
            audioManager.PlaySFX(audioManager.SwordSlash);
            Attack();
            nextAttackTime = Time.time + attackCooldown;
            MobileInput.attack = false;
        }
    }

    // ===============================
    // NEW: HITBOX IKUT FLIP
    // ===============================
    public void UpdateAttackPointDirection(bool facingRight)
    {
        if (facingRight)
            attackPoint.localPosition = attackPointRight;
        else
            attackPoint.localPosition = attackPointLeft;
    }


    void Attack()
    {
        movement.isAttacking = true;
        anim.SetTrigger("IsAttacking");
    }

    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()
                 .TakeDamage(damage, transform.position);
        }
    }

    public void EndAttack()
    {
        movement.isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
