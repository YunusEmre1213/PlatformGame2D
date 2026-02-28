using UnityEngine;

public class SonsuzArkaplan : MonoBehaviour
{
    // Hangi nesnenin hareketini takip edeceðimizi belirleyen deðiþken
    public Transform kamera;

    // Yüzdesel hareket hýzý (0.1f = %10)
    public float parallaxEtkisi = 0.5f;

    // Arka planýn geniþliði (piksel deðil, Unity birimi)
    private float genislik;

    // Arka planýn baþlangýç pozisyonu
    private Vector3 baslangicPozisyonu;

    void Start()
    {
        // Baþlangýç pozisyonunu ve geniþliði al
        baslangicPozisyonu = transform.position;
        // Sprite'ýn geniþliðini al, bu sahne üzerinde otomatik olarak hesaplanýr
        genislik = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Kameranýn ne kadar hareket ettiðini bul
        float mesafe = kamera.position.x * parallaxEtkisi;

        // Sonsuz döngü için tekrar etme miktarýný hesapla
        float tekrarMiktari = kamera.position.x * (1 - parallaxEtkisi);

        // Arka planýn yeni pozisyonunu ayarla
        Vector3 yeniPozisyon = new Vector3(baslangicPozisyonu.x + mesafe, transform.position.y, transform.position.z);
        transform.position = yeniPozisyon;

        // Arka planý yenileme kontrolü
        // Eðer kamera bir sonraki arka planýn pozisyonuna ulaþtýysa, en baþa gönder
        if (tekrarMiktari > baslangicPozisyonu.x + genislik)
        {
            baslangicPozisyonu.x += genislik;
        }
        else if (tekrarMiktari < baslangicPozisyonu.x - genislik)
        {
            baslangicPozisyonu.x -= genislik;
        }
    }
}