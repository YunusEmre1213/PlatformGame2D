using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // D��man�n h�z�
    public float speed = 2f;

    // Devriye gezece�i noktalar
    public Transform patrolPointA;
    public Transform patrolPointB;

    // D��man�n verdi�i hasar miktar�
    public int damageAmount = 10;

    // Oyuncunun Rigidbody2D bile�eni, stomp kontrol� i�in
    public Rigidbody2D playerRb;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPatrolPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Ba�lang��ta B noktas�na do�ru hareket et
        currentPatrolPoint = patrolPointB;
    }

    private void Update()
    {
        // Hedef noktaya do�ru hareket et
        Vector2 targetPosition = new Vector2(currentPatrolPoint.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Hedef noktaya ula�t� m� kontrol et
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            // Y�n de�i�tir
            if (currentPatrolPoint == patrolPointB)
            {
                currentPatrolPoint = patrolPointA;
            }
            else
            {
                currentPatrolPoint = patrolPointB;
            }

            // D��man� d�nd�r
            Flip();
        }

        // Animasyon i�in
        anim.SetBool("isWalking", true);
    }

    // YEN� EKLENEN KISIM: Karakterle fiziksel �arp��ma alg�lamas�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // E�er temas eden nesne "Player" tag'ine sahipse
        if (collision.gameObject.CompareTag("Player"))
        {
            // E�er oyuncu d��man�n �zerine d���yorsa (y eksenindeki h�z� negatifse)
            // Bu, 'kafas�ndan z�plama' sald�r�s�n� alg�lar.
            if (playerRb.linearVelocity.y < 0)
            {
                // D��man� yok et
                Destroy(gameObject);
                Debug.Log("D��man yok edildi!");
            }
            else
            {
                // De�ilse, oyuncuya hasar ver (yandan �arpm��sa)
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    Debug.Log("Oyuncu d��mandan hasar ald�!");
                }
            }
        }
    }

    private void Flip()
    {
        // D��man�n X eksenindeki y�n�n� ters �evir
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}