using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBarFillImage;
    public float fillSpeed = 0.5f;

    // Yeni: Saðlýk deðiþimini yavaþça yansýtan Coroutine
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Hedef doluluk miktarýný hesapla
        float targetFillAmount = currentHealth / maxHealth;

        // Dolum iþlemini baþlat
        StartCoroutine(SmoothFill(targetFillAmount));
    }

    private IEnumerator SmoothFill(float targetFillAmount)
    {
        float startFillAmount = healthBarFillImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * fillSpeed;
            healthBarFillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime);
            yield return null;
        }

        // Animasyonun sonunda tam hedef deðere ulaþmasýný saðla
        healthBarFillImage.fillAmount = targetFillAmount;
    }
}