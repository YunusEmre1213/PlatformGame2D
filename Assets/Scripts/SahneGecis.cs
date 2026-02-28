using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SahneGecis : MonoBehaviour
{
    public string sonrakiSahneAdi;

    public Image fadeImage;

    public float fadeSure = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SahneGecisiCoroutine());
        }
    }

    private IEnumerator SahneGecisiCoroutine()
    {
        yield return StartCoroutine(Fade(1, fadeSure));

        SceneManager.LoadScene(sonrakiSahneAdi);

        // Yeni sahnede de ayný script kullanýlacaksa, paneli görünmez yap.
        // Bu kýsým, yeni sahnenizde de bu script'i kullanacaksanýz önemlidir.
        // Fade(0, fadeSure); // Bu satýrý yeni sahnenizdeki baþlangýç koduna ekleyebilirsiniz.
    }

    private IEnumerator Fade(float hedefAlfa, float sure)
    {
        float mevcutSure = 0;
        Color renk = fadeImage.color;
        float baslangicAlfa = renk.a;

        while (mevcutSure < sure)
        {
            mevcutSure += Time.deltaTime;
            float yeniAlfa = Mathf.Lerp(baslangicAlfa, hedefAlfa, mevcutSure / sure);
            fadeImage.color = new Color(renk.r, renk.g, renk.b, yeniAlfa);
            yield return null; // Bir sonraki kareye kadar bekle.
        }

        fadeImage.color = new Color(renk.r, renk.g, renk.b, hedefAlfa);
    }
}