using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundCycler : MonoBehaviour
{
    public Sprite[] backgrounds;      // Inspector'da 4 resmi sýrayla sürükle
    public float secondsBetween = 3f; // otomatik geçiþ istemezsen 0 yap
    private SpriteRenderer sr;
    private int index = 0;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (backgrounds != null && backgrounds.Length > 0) sr.sprite = backgrounds[0];
    }

    void Start()
    {
        if (backgrounds != null && backgrounds.Length > 1 && secondsBetween > 0f)
        {
            StartCoroutine(CycleCoroutine());
        }
    }

    IEnumerator CycleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetween);
            index = (index + 1) % backgrounds.Length;
            sr.sprite = backgrounds[index];
        }
    }

    // Butona veya olay tetikleyiciye baðlamak için:
    public void NextBackground()
    {
        if (backgrounds == null || backgrounds.Length == 0) return;
        index = (index + 1) % backgrounds.Length;
        sr.sprite = backgrounds[index];
    }

    public void SetBackground(int idx)
    {
        if (backgrounds == null || idx < 0 || idx >= backgrounds.Length) return;
        index = idx;
        sr.sprite = backgrounds[index];
    }
}
