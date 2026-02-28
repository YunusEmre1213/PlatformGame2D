using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    // Saldýrý menzilini belirleyen Transform
    [SerializeField] private Transform attackPoint;
    // Saldýrý menzili yarýçapý
    [SerializeField] private float attackRange = 0.5f;
    // Hangi katmana (layer) hasar verileceðini belirler (Player Layer'ý seçilmeli)
    [SerializeField] private LayerMask playerLayer;

    // Yöntem 1: Animation Event tarafýndan çaðrýlacak metot (Parametresiz)
    public void DealDamage() // Artýk parametre almýyor
    {
        // attackPoint etrafýndaki attackRange içindeki Player'larý tespit et
        // Physics2D.OverlapCircleAll metodu en iyi yöntemdir.
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D playerCollider in hitPlayers)
        {
            // Oyuncunun PlayerHealth script'ine ulaþ
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Oyuncuya hasar ver (Bu satýr artýk null hatasý vermeyecek)
                playerHealth.TakeDamage(attackDamage);
                // Ýlk vuruþtan sonra döngüden çýkýlabilir, tek hedefe vuruluyorsa.
                // break; 
            }
        }
    }

    // Geliþtirme/Test amaçlý menzili görme (Ýsteðe baðlý)
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}