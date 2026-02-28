using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject keyPromptUI; // Bu bir Canvas içindeki Text/TextMeshPro nesnesi olmalı

    // INSPECTOR'DAN KONTROL EDİLECEK YENİ DEĞİŞKENLER
    [Tooltip("Sandıktan düşecek coin sayısı.")]
    public int numberOfCoins = 5;
    [Tooltip("Coin'lerin ilk fırlama gücü.")]
    public float coinBurstForce = 10f;
    [Tooltip("Sandığın sahneden silinmeden önce beklenecek süre.")]
    public float chestDestroyDelay = 2f;
    [Tooltip("Coin'ler yere düştüğünde uygulanacak fizik materyali.")]
    public PhysicsMaterial2D landedMaterial;

    private bool isOpened = false;
    private Coroutine currentPromptCoroutine;

    private void OnValidate()
    {
        if (landedMaterial == null)
        {
            Debug.LogWarning("ChestController: landedMaterial atanmamış! Lütfen bir PhysicsMaterial2D atayın.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player"))
        {
            if (KeyController.hasKey)
            {
                OpenChest();
                // Sandık açılacağı için varsa uyarıyı hemen kapat
                HideKeyPrompt();
            }
            else
            {
                // Anahtar yoksa uyarıyı göster
                ShowKeyPrompt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Karakter sandıktan uzaklaştığında metni hemen gizle
        if (collision.CompareTag("Player"))
        {
            HideKeyPrompt();
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        Debug.Log("Sandık açılıyor!");
        SpawnCoin();

        // Coin'ler fırladıktan sonra sandığı sahneden sil
        StartCoroutine(DestroyChestAfterDelay());
    }

    private void ShowKeyPrompt()
    {
        if (keyPromptUI != null)
        {
            keyPromptUI.SetActive(true);
            Debug.Log("Anahtarı bulun!");

            // Mevcut bir coroutine varsa durdur
            if (currentPromptCoroutine != null)
            {
                StopCoroutine(currentPromptCoroutine);
            }

            // Yeni coroutine'i başlat
            currentPromptCoroutine = StartCoroutine(HideKeyPromptAfterDelay(2f));
        }
    }

    private void HideKeyPrompt()
    {
        if (keyPromptUI != null)
        {
            keyPromptUI.SetActive(false);
        }

        // Coroutine'i durdur
        if (currentPromptCoroutine != null)
        {
            StopCoroutine(currentPromptCoroutine);
            currentPromptCoroutine = null;
        }
    }

    private IEnumerator HideKeyPromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideKeyPrompt();
    }

    private void SpawnCoin()
    {
        // Coinleri, sandığın pozisyonundan rastgele bir yönde fırlat.
        // Artık boş nesneye ihtiyacımız yok, direkt zemin collider'ı ile etkileşime geçeceğiz.
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.2f, 0);

            GameObject coinInstance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            // Yeni oluşturulan coin'in Rigidbody2D ve Collider2D bileşenlerini al
            Rigidbody2D rb = coinInstance.GetComponent<Rigidbody2D>();
            Collider2D coinCollider = coinInstance.GetComponent<Collider2D>();

            if (rb != null)
            {
                // Coin'in yerçekimi değerini 1 yaparak yere düşmesini sağla
                rb.gravityScale = 1f;

                // Coin'in çarpışmasını mümkün kılmak için IsTrigger'ı kapat
                if (coinCollider != null)
                {
                    coinCollider.isTrigger = false;
                }

                // Coin'e itme (impulse) kuvveti uygula
                float randomX = Random.Range(-1f, 1f);
                Vector2 burstDirection = new Vector2(randomX, 1f).normalized;
                rb.AddForce(burstDirection * coinBurstForce, ForceMode2D.Impulse);

                // Coin'in yere inmesini beklemek ve IsTrigger'ı tekrar açmak için Coroutine başlat.
                StartCoroutine(WaitForCoinToLand(coinInstance));
            }
        }
    }

    // Coin yere düştükten sonra onu toplanabilir hale getiren Coroutine.
    private IEnumerator WaitForCoinToLand(GameObject coin)
    {
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
        Collider2D coinCollider = coin.GetComponent<Collider2D>();

        // Coin yere temas edene kadar bekle.
        // Bu, hızın düşmesini veya yerle teması kontrol eder.
        yield return new WaitWhile(() => rb.linearVelocity.magnitude > 0.05f || rb.angularVelocity > 0.05f);

        // Coin'in hızını ve dönüşünü sıfırla.
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Coin'i kinematik yaparak tamamen durmasını sağla.
        rb.isKinematic = true;

        // Coin'i tekrar toplanabilir hale getir.
        if (coinCollider != null)
        {
            coinCollider.isTrigger = true;
            // Duruş sürtünmesi için fizik materyalini uygula.
            coinCollider.sharedMaterial = landedMaterial;
        }
    }

    private IEnumerator DestroyChestAfterDelay()
    {
        yield return new WaitForSeconds(chestDestroyDelay);
        Destroy(gameObject);
    }
}
