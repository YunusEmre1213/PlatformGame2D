using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadePanel;
    public float fadeSpeed = 1f;

    // Bu fonksiyon ile sahne geçiþini baþlatacaðýz
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    // Sahneye geçmeden önce ekranýn kararmasýný saðlayan IEnumerator
    IEnumerator FadeOut(string sceneName)
    {
        float alpha = 0f;
        fadePanel.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null; // Bir sonraki karede devam et
        }

        // Ekran tamamen karardýktan sonra sahneyi yükle
        SceneManager.LoadScene(sceneName);
    }
}