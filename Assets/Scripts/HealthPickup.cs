using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 10; // Ne kadar can yenileyeceði

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bu satýrý buraya ekle
            Debug.Log("Can iksiri, oyuncuyla temas etti!");

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log("Can iksiri alýndý! Can yenilendi.");
            }
            Destroy(gameObject);
        }
    }
}