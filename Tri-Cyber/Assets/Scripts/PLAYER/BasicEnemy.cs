using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do inimigo
    public float changeDirectionTime = 2f; // Tempo em segundos para mudar de direção
    public float damageAmount = 1; // Quantidade de dano causado ao jogador
    
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private bool isMovingRight = true;
    private float timer;
    private bool hasCollided = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeDirectionTime;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        // Movimento do inimigo
        if (isMovingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        // Contagem regressiva para mudar de direção
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChangeDirection();
            timer = changeDirectionTime;
        }
    }

    private void ChangeDirection()
    {
        isMovingRight = !isMovingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            hasCollided = true;
            playerMovement.playerHealth -= damageAmount;

            if (playerMovement.playerHealth <= 0)
            {
                // q
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = false;
        }
    }
}