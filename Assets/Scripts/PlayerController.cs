using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;

    private Animator anim;
    private PlayerHealth playerHealth;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private float knockbackTimer;

    [Header("Saldırı")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    // YENİ EKLENEN BIÇAK DEĞİŞKENLERİ
    // ----------------------------------------------------------------
    [Header("Bıçak Fırlatma")]
    public GameObject knifePrefab; // Fırlatılacak bıçağın Prefab'ı (Inspector'da atayın)
    public Transform throwPoint;   // Bıçağın karakterin neresinden fırlatılacağını belirten Transform
    public float throwForce = 15f; // Bıçağın fırlatma kuvveti

    // Bıçak sayısını Private yaptık, Inspector'da görmeyi sağlarız.
    [SerializeField]
    private int knifeCount = 0;
    // ----------------------------------------------------------------


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        // Oyun başladığında UI'ı başlatmak için UIManager'a mevcut bıçak sayısını gönder.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKnifeCount(knifeCount);
        }
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        anim.SetFloat("HareketHizi", Mathf.Abs(horizontalInput));
        anim.SetBool("yerdeMi", isGrounded);

        YonuDegistir();
    }

    // MEKANİK 1: BIÇAK SAYISINI ARTIRMA METODU
    // Bu metot, PickUpItem.cs script'i tarafından çağrılır.
    public void IncreaseKnifeCount(int amount)
    {
        knifeCount += amount;

        // UI Yöneticiye güncel sayıyı gönder
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKnifeCount(knifeCount);
        }

        Debug.Log($"Bıçak alındı! Toplam: {knifeCount}");
    }


    // MEKANİK 2: BIÇAĞI FIRLATMA (THROW) INPUT VE METODU
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed && knifeCount > 0)
        {
            ThrowKnife();
        }
    }

    void ThrowKnife()
    {
        if (knifePrefab == null || throwPoint == null)
        {
            Debug.LogError("Knife Prefab veya Throw Point atanmamış! Fırlatılamıyor.");
            return;
        }

        // Bıçak sayısını azalt
        knifeCount--;

        // UI Yöneticiye güncel sayıyı gönder
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKnifeCount(knifeCount);
        }

        // 1. Bıçağın bir örneğini (Instance) oluştur
        GameObject knife = Instantiate(knifePrefab, throwPoint.position, Quaternion.identity);

        // 2. Fırlatma yönünü belirle (karakterin baktığı yöne)
        float directionX = transform.localScale.x < 0 ? 1f : -1f; // Sağ (+1) veya Sol (-1)
        Vector2 throwDirection = new Vector2(directionX, 0).normalized;

        // 3. Bıçağın hareket script'ini çağır
        KnifeScript knifeScript = knife.GetComponent<KnifeScript>();

        if (knifeScript != null)
        {
            knifeScript.Launch(throwDirection * throwForce);
        }
        else
        {
            Debug.LogError("Fırlatılan bıçak prefab'ında KnifeScript bulunamadı!");
        }

    }

    // MEVCUT KODLAR (Hareket, Zıplama, Yön Değiştirme, Saldırı...)
    // ----------------------------------------------------------------

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void YonuDegistir()
    {
        Vector2 geciciSkale = transform.localScale;
        if (rb.linearVelocity.x > 0.1f)
        {
            geciciSkale.x = -1f; // Sağa bak
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            geciciSkale.x = 1f; // Sola bak
        }
        transform.localScale = geciciSkale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.spikeDamage);

                knockbackTimer = knockbackDuration;
                Vector2 knockbackDirection = (transform.position.x - other.transform.position.x > 0) ? Vector2.right : Vector2.left;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log(enemy.name + " hasar aldı!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}