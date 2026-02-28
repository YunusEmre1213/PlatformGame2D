using UnityEngine;
using TMPro; // TextMeshPro kullanýyorsanýz bu kütüphaneyi ekleyin

public class CoinSystem : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int coinCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;

            UpdateCoinText();

            Destroy(other.gameObject);
        }
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }
}
