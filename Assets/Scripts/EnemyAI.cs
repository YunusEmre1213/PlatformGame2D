using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // H�z ve menzil ayarlar�
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1.5f;

    // Hedef (oyuncu) referans�
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        // Gerekli bile�enleri al
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // Oyuncuyu bul (tag'i "Player" oldu�unu varsayal�m)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Oyuncunun ne kadar uzakta oldu�unu hesapla
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // 1. Sald�r�
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // 2. Takip Etme
            ChasePlayer();
        }
        else
        {
            // 3. Bo� Durma (Idle)
            Idle();
        }

        // Y�n� �evirme (Sprite'� hedefe g�re d�nd�rme)
        FlipSprite();
    }

    void ChasePlayer()
    {
        // Oyuncuya do�ru hareket et
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Animasyonu ayarla
        anim.SetBool("IsRunning", true);
    }

    void Attack()
    {
        // Hareket etmeyi durdur
        rb.linearVelocity = Vector2.zero;

        // Sald�r� animasyonunu tetikle
        anim.SetTrigger("Attack");

        // Animasyonu durdur
        anim.SetBool("IsRunning", false);

        // Not: Ger�ek hasar verme mant��� Attack animasyonunun belirli bir karesinde (Animation Event ile) yap�lmal�d�r.
    }

    void Idle()
    {
        // Hareket etmeyi durdur
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Animasyonu ayarla
        anim.SetBool("IsRunning", false);
    }

    void FlipSprite()
    {
        // Hareket y�n�ne g�re sprite'� d�nd�r
        if (rb.linearVelocity.x != 0)
        {
            if (rb.linearVelocity.x > 0)
            {
                // Sa� tarafa bak
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else // rb.velocity.x < 0
            {
                // Sol tarafa bak (Sprite'� ters �evir)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
}