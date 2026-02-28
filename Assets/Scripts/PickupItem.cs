using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Toplama Ayarlarý")]
    public string itemType = "Knife"; // (Opsiyonel: Farklý eþyalar için tutulabilir)
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Temas eden nesne "Player" etiketi taþýyor mu?
        if (other.CompareTag("Player"))
        {
            // PlayerController script'ine ulaþmaya çalýþ
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // *** HATA DÜZELTME BURADA YAPILDI ***
                // PlayerController'daki IncreaseKnifeCount metodunu çaðýr
                playerController.IncreaseKnifeCount(amount);

                // Toplanabilir öðeyi yok et
                Destroy(gameObject);
            }
        }
    }
}