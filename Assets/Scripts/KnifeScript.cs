using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float ignoreCollisionTime = 0.1f; 

    void Start()
    {
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();
        Collider2D knifeCollider = GetComponent<Collider2D>();

        if (playerCollider != null && knifeCollider != null)
        {
            Physics2D.IgnoreCollision(knifeCollider, playerCollider, true);

            Invoke("ReEnableCollision", ignoreCollisionTime);
        }
    }

    void ReEnableCollision()
    {
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();
        Collider2D knifeCollider = GetComponent<Collider2D>();

        if (playerCollider != null && knifeCollider != null)
        {
            Physics2D.IgnoreCollision(knifeCollider, playerCollider, false);
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

 
    public void Launch(Vector2 direction)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.gravityScale = 0f;

        rb.linearVelocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int knifeDamage = 10; 

        if (other.CompareTag("Enemy"))
        {
            EnemyBirdHealth enemyHealth = other.GetComponent<EnemyBirdHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(knifeDamage);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Ground"))
        {

            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; 

            Destroy(gameObject, 3f);
        }
    }
}