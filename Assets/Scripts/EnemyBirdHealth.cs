using UnityEngine;

public class EnemyBirdHealth : MonoBehaviour
{
    // Yaratýðýn baþlangýç caný. (Býçak hasarýný 10 kabul edersek, 2 vuruþ için 20 olmalý)
    [Header("Saðlýk Ayarlarý")]
    public int maxHealth = 20;

    // Yaratýðýn mevcut caný
    private int currentHealth;

    [Header("Duyusal Ayarlar")]
    public GameObject deathEffectPrefab; // (Opsiyonel) Yaratýk öldüðünde patlama efekti

    void Awake()
    {
        // Oyun baþladýðýnda caný maksimuma ayarla
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Býçak veya baþka bir kaynaktan gelen hasarý iþler.
    /// </summary>
    /// <param name="damageAmount">Alýnan hasar miktarý.</param>
    public void TakeDamage(int damageAmount)
    {
        // Caný azalt
        currentHealth -= damageAmount;

        Debug.Log(gameObject.name + " hasar aldý. Kalan Can: " + currentHealth);

        // Can sýfýr veya altýna düþtüyse Ölüm metodunu çaðýr
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // (Opsiyonel: Hasar aldýðýnda kýsa bir süreliðine kýrmýzýya dönme, ses efekti vb.)
        }
    }

    /// <summary>
    /// Yaratýðýn yok olmasýný saðlar.
    /// </summary>
    void Die()
    {
        Debug.Log(gameObject.name + " yok edildi!");

        // Ölüm efektini oluþtur (eðer atanmýþsa)
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Yaratýk nesnesini sahneden kaldýr
        Destroy(gameObject);
    }
}
