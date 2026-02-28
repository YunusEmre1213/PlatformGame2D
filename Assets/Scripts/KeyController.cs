using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Karakterin anahtara sahip olup olmadýðýný tutan statik bir deðiþken.
    // Static olduðu için, baþka script'lerden doðrudan eriþilebilir.
    public static bool hasKey = false;

    // UI text'ini gizleme/gösterme amaçlý
    public GameObject uiPrompt;

    // Oyuncu anahtara dokunduðunda çalýþacak metot.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Temas eden nesnenin "Key" etiketi olup olmadýðýný kontrol et.
        // Bu yüzden anahtar nesnesine "Key" etiketi vermeyi unutma.
        if (collision.CompareTag("Key"))
        {
            hasKey = true; // Anahtarý aldýk.
            Debug.Log("Anahtar alýndý!");

            // UI uyarýsýný kapat (eðer açýksa).
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
            }

            // Anahtar nesnesini sahnede görünmez yap veya yok et.
            Destroy(collision.gameObject);
        }
    }
}