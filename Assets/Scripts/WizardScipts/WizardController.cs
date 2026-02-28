using UnityEngine;

public class WizardController : MonoBehaviour
{
    // === UNITY EDITOR AYARLARI ===
    [Header("Görüþ Alaný Ayarlarý (Raycast)")]
    public float sightRange = 8f;       // Wizard'ýn görüþ mesafesi
    public LayerMask playerLayer;       // Oyuncunun Layer'ý
    public LayerMask obstacleLayer;     // Engellerin Layer'ý (Duvarlar vb.)
    public Transform raycastPoint;      // Raycast'in fýrlatýlacaðý nokta

    [Header("Saldýrý Ayarlarý")]
    public GameObject spellPrefab;      // Fýrlatýlacak Büyü Prefab'ý
    public Transform firePoint;         // Büyünün oluþturulacaðý pozisyon
    public float attackRange = 5f;      // Büyü atma mesafesi
    public float timeBetweenAttacks = 2f; // Ýki saldýrý arasýndaki bekleme süresi
    private float nextAttackTime = 0f;

    [Header("Görsel ve Durum Ayarlarý")]
    public bool isFacingRight = true;   // Wizard þu an saða mý bakýyor?

    // === ÖZEL DEÐÝÞKENLER ===
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform targetPlayer;

    // Animator parametreleri için hash deðerleri (performans için)
    private readonly int AttackTriggerHash = Animator.StringToHash("AttackTrigger");
    private readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    // ====================================
    // UNITY METOTLARI
    // ====================================

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Varsayýlan atama kontrolleri
        if (raycastPoint == null) raycastPoint = transform;
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        LookForPlayer();

        // Eðer oyuncu görüþ alanýndaysa
        if (targetPlayer != null)
        {
            FlipTowardsTarget(targetPlayer.position);
            HandleCombat();
        }
        else
        {
            // Oyuncu görüþ alanýnda deðil, Idle durumuna geç
            animator.SetBool(IsAttackingHash, false);
        }
    }

    // ====================================
    // GÖRÜÞ VE HAREKET
    // ====================================

    void LookForPlayer()
    {
        // Wizard'ýn baktýðý yön
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // Raycast fýrlat: Hem Oyuncuyu hem de Engelleri algýla
        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, direction, sightRange, playerLayer | obstacleLayer);

        // Debug için Ray'i çiz (Scene penceresinde görünür)
        Debug.DrawRay(raycastPoint.position, direction * sightRange, Color.red);

        if (hit.collider != null)
        {
            // Raycast bir þeye çarptý

            // Eðer çarpýlan þey oyuncuysa VE arada bir engel yoksa
            // Hit'in layer'ý oyuncu layer'ýnda mý?
            if (hit.collider.CompareTag("Player") &&
                ((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                targetPlayer = hit.transform;
                return; // Oyuncu görüldü
            }
        }

        // Oyuncu görülmedi VEYA arada engel var
        targetPlayer = null;
    }

    /// <summary>
    /// Wizard'ý hedefin pozisyonuna doðru döndürür (Görsel Flip).
    /// </summary>
    void FlipTowardsTarget(Vector3 targetPosition)
    {
        float direction = targetPosition.x - transform.position.x;

        if (direction > 0 && !isFacingRight) // Hedef saðda, Wizard sola bakýyor -> Saða dön
        {
            Flip();
        }
        else if (direction < 0 && isFacingRight) // Hedef solda, Wizard saða bakýyor -> Sola dön
        {
            Flip();
        }
    }

    /// <summary>
    /// Wizard'ýn yönünü tersine çevirir.
    /// </summary>
    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Sprite'ý yatay eksende ters çevir
        spriteRenderer.flipX = !isFacingRight;
    }

    // ====================================
    // SALDIRI MANTIÐI
    // ====================================

    void HandleCombat()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer <= attackRange)
        {
            // Saldýrý Mesafesinde
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + timeBetweenAttacks;
            }
            // Bekleme süresi içindeyken veya Attack animasyonu oynarken IsAttacking true kalýr
            animator.SetBool(IsAttackingHash, true);
        }
        else
        {
            // Saldýrý menzilinde deðil
            animator.SetBool(IsAttackingHash, false);
        }
    }

    // Saldýrýyý baþlatýr (Animasyon tetiklenir)
    void Attack()
    {
        animator.SetTrigger(AttackTriggerHash);
        // Büyü fýrlatma (ShootSpell) animasyon olayý ile çaðrýlacak.
    }

    // Bu metot, Unity Animation Event ile Attack animasyonu üzerinden çaðrýlmalýdýr.
    public void ShootSpell()
    {
        if (targetPlayer == null || spellPrefab == null) return;

        // Büyünün fýrlatýlacaðý yönü hesapla (Wizard'ýn baktýðý yönde)
        Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;

        // Büyü prefab'ýný oluþtur
        GameObject newSpell = Instantiate(spellPrefab, firePoint.position, Quaternion.identity);

        // Büyünün yönünü ayarla (SpellController içinde transform.right kullanýldýðý için)
        newSpell.transform.right = direction;
    }
}