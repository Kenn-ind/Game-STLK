using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))] // pastikan ada Rigidbody2D
public class Player : MonoBehaviour
{
    [Header("Selection")]
    public GameObject ulang;
    public GameObject kembali;
    public GameObject lanjut;
    public GameObject pause;
    public GameObject DarkPanel1;
    public GameObject hearts5;
    public GameObject hearts4;
    public GameObject hearts3;
    public GameObject hearts2;
    public GameObject hearts1;
    public GameObject hearts0;
    public Image menang;
    public Image kalah;
    PlayerAttack attack;


    [Header("Score")]
    public int skor = 0;
    
    [Header("Status Player")]
    public int maxHP = 5;
    private int currentHP;

    [Header("UI TMP")]
    public TextMeshProUGUI hpText;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float jumpForce = 7f; // kekuatan loncat
    public bool canMove = true;   // <-- INI YANG DIPAKAI DIALOG
    public bool isAttacking = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Damage System")]
    public float knockbackForce = 5f;
    public float invincibleTime = 1.2f;
    private bool isInvincible = false;
    private bool isHurt = false;

    //[Header("Layer Settings")]
    //public string playerLayer = "Player";
    //public string enemyLayer = "musuh";

    public bool isGrounded = false; // cek apakah player menyentuh tanah

    AudioManager audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        attack = GetComponent<PlayerAttack>();

        currentHP = maxHP;
        UpdateHPUI();
    }

    private void Update()
    {
        if (!canMove) return;
        if (isInvincible) return;

        // ===== JUMP =====
        bool jumpInput = Input.GetKeyDown(KeyCode.Space) || MobileInput.jump;

        if (jumpInput && isGrounded)
        {
            audioManager.PlaySFX(audioManager.Jump);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            MobileInput.jump = false;
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isHurt) return;

        if (isAttacking)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // ===== GERAK =====

        float horizontalInput = Input.GetAxis("Horizontal");

        if (MobileInput.horizontal != 0)
            horizontalInput = MobileInput.horizontal;

        float x = horizontalInput * moveSpeed * Time.deltaTime;
        transform.Translate(x, 0, 0);

        if (x != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        animator.SetBool("IsJumping", !isGrounded);

        // ===== FLIP CHARACTER (AMAN UNTUK HITBOX) =====
        if (x > 0)
        {
            spriteRenderer.flipX = false;
            attack.UpdateAttackPointDirection(true);
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = true;
            attack.UpdateAttackPointDirection(false);
        }



        if (currentHP == 4)
        {
            hearts5.SetActive(false);
        }
        else if (currentHP == 3)
        {
            hearts4.SetActive(false);
        }
        else if (currentHP == 2)
        {
            hearts3.SetActive(false);
        }
        else if (currentHP == 1)
        {
            hearts2.SetActive(false);
        }

        if (skor == 5 && GameObject.FindGameObjectsWithTag("bahaya").Length == 0) //kondisi menang
        {
            //skor3.gameObject.SetActive(true);
            audioManager.PlaySFX(audioManager.Win);
            menang.gameObject.SetActive(true);
            lanjut.SetActive(true);
            kembali.SetActive(true);
            pause.SetActive(false);
            DarkPanel1.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // cek apakah player nyentuh tanah (tag = "Ground")
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("bahaya"))
        {
            if (!isInvincible)
            {
                audioManager.PlaySFX(audioManager.Hurt);
                animator.SetTrigger("IsHurt"); // <<< TAMBAHAN INI
                TakeDamage(1);
                ApplyKnockback(collision.transform);
            }
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            audioManager.PlaySFX(audioManager.Scoring);

        }
    }

    private void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (currentHP <= 0) //player modar
        {
            // Play animasi mati
            animator.SetTrigger("IsDead");

            // UI tetap jalan
            hearts1.SetActive(false);
            // Hancurkan setelah animasi selesai
            StartCoroutine(GameOverDelay());
        }
    }

    private System.Collections.IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 0f;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        audioManager.PlaySFX(audioManager.GameOver);
        kalah.gameObject.SetActive(true);
        ulang.SetActive(true);
        kembali.SetActive(true);
        pause.SetActive(false);
        DarkPanel1.SetActive(true);
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = $"HP: ";
        //{ currentHP}
    }

    private void ApplyKnockback(Transform enemy)
    {
        if (isHurt) return;

        isHurt = true;
        isInvincible = true;

        // arah knockback
        float direction = transform.position.x < enemy.position.x ? -1 : 1;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * knockbackForce, 4f), ForceMode2D.Impulse);

        // visual feedback optional
        StartCoroutine(InvincibleCoroutine());
        StartCoroutine(HurtCoroutine());
    }

    private System.Collections.IEnumerator InvincibleCoroutine()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private System.Collections.IEnumerator HurtCoroutine()
    {
        yield return new WaitForSeconds(0.7f); // durasi animasi hurt
        isHurt = false;
    }

}
