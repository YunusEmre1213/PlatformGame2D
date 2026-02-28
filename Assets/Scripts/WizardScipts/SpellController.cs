using UnityEngine;

public class SpellController : MonoBehaviour
{
    public float spellSpeed = 7f; // Büyünün hızı
    public int damage = 10;       // Vereceği hasar miktarı
    public float lifetime = 3f;   // Büyünün yok olma süresi

    void Start()
    {
        // Büyüyü hemen fırlatma yönünde hareket ettir
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * spellSpeed;

        // Belirli bir süre sonra kendini yok et
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Oyuncuya çarptıysa
        if (other.CompareTag("Player"))
        {
            // PlayerHealth script'ini al
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Hasar ver
            }

            // Büyüyü yok et
            Destroy(gameObject);
        }
        // Başka bir engele (Duvar, Zemin vb.) çarptıysa
        else if (!other.CompareTag("Wizard") && !other.CompareTag("IgnoreCollision"))
        {
            // Wizard'ın kendisine veya çarpışmayı yoksaydığımız objelere çarpmadıysa yok et
            Destroy(gameObject);
        }
    }
}