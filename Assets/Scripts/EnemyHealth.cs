using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    private Animator anim;
    // NOT: Rigidbody'yi durdurmak i�in gerekebilir
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Rigidbody'yi al
    }

    // D��man�n hasar almas� i�in �a�r�lacak metot
    public void TakeDamage(int damage)
    {
        // �l�yse tekrar hasar almas�n� engelle
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " hasar ald�! Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // HASAR ALMA ANIMASYONUNU TET�KLE
            if (anim != null)
            {
                anim.SetTrigger("TakeHit"); // Animator'da tan�mlad���m�z Trigger
            }
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " �ld�!");

        // �L�M ANIMASYONUNU TET�KLE
        if (anim != null)
        {
            anim.SetTrigger("Die"); // Animator'da tan�mlad���m�z Trigger
        }

        // �ld���nde hareket etmesini ve �arp��ma yapmas�n� durdur
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Hareketi durdur
            rb.isKinematic = true; // Fizik etkile�imini durdur
        }
        // Collider'� da devre d��� b�rak
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Animasyon bittikten sonra objeyi yok et
        // Buradaki 2 saniye animasyon uzunlu�una g�re ayarlanmal�d�r
        Destroy(gameObject, 2f);
    }
}