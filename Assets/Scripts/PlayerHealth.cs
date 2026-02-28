using System.Collections;

using UnityEngine;



public class PlayerHealth : MonoBehaviour

{

    public HealthBarUI healthBarUI; // Can barý scriptine referans

    public int maxHealth = 100;

    public int currentHealth;



    public int spikeDamage = 20;



    private Animator anim;



    public float invincibilityDuration = 1f; // Dokunulmazlýk süresi

    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;



    void Awake()

    {

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

    }



    void Start()

    {

        currentHealth = maxHealth;

        // Oyun baþladýðýnda can barýný tam dolu olarak ayarla

        if (healthBarUI != null)

        {

            healthBarUI.UpdateHealthBar(currentHealth, maxHealth);

        }

    }



    public void TakeDamage(int damage)

    {

        // Eðer karakter zaten dokunulmazsa, hasar almasýný engelle

        if (isInvincible) return;



        // Hasarý can deðerinden düþür

        currentHealth -= damage;

        Debug.Log("Hasar alýndý! Kalan can: " + currentHealth);



        // Can barýný mevcut can deðerine göre güncelle

        if (healthBarUI != null)

        {

            healthBarUI.UpdateHealthBar(currentHealth, maxHealth);

        }



        // Hasar alma animasyonunu tetikle

        if (anim != null)

        {

            anim.SetTrigger("HasarAldi");

        }



        // Dokunulmazlýk coroutine'ini baþlat

        StartCoroutine(BecomeInvincible());



        // Can sýfýrýn altýna düþerse karakteri öldür

        if (currentHealth <= 0)

        {

            Die();

        }

    }



    // YENÝ METOT: Can artýrmak için

    public void Heal(int amount)

    {

        Debug.Log("Heal metodu çaðrýldý.");

        currentHealth += amount;

        // Canýn maksimum deðeri geçmesini engelle

        if (currentHealth > maxHealth)

        {

            currentHealth = maxHealth;

        }



        // Can barýný güncelle

        if (healthBarUI != null)

        {

            healthBarUI.UpdateHealthBar(currentHealth, maxHealth);

        }

    }



    IEnumerator BecomeInvincible()

    {

        isInvincible = true;

        // Yanýp sönme efekti

        float blinkTimer = 0;

        float blinkRate = 0.1f;

        while (blinkTimer < invincibilityDuration)

        {

            spriteRenderer.enabled = !spriteRenderer.enabled; // Sprite'ý göster/gizle

            yield return new WaitForSeconds(blinkRate);

            blinkTimer += blinkRate;

        }



        spriteRenderer.enabled = true; // Efekt bitince sprite'ý görünür yap

        isInvincible = false;

    }



    void Die()

    {

        Debug.Log("Karakter öldü!");

        // Karakter öldüðünde yapýlacak iþlemleri buraya ekleyebilirsiniz (örneðin, sahneyi yeniden baþlatmak).

    }
}