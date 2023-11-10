using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private float horizontalMove = 0f;
    public float runSpeed = 40f;
    private bool jump = false;
    public Animator animator;
    private int transition = 0;
    public GameObject bala;
    public Transform balaPoint;
    public GameObject balaEspecial;
    public RectTransform healthBar;

    private bool canDash = true;
    public float dashForce = 100f;
    public float dashDuration = 0.2f;
    private float dashTimer = 0f;

    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    private float invulnerabilityTimer = 0f;

    private bool canShoot = true;
    public float shootCooldown = 1f;

    public float playerHealth = 5;
    private AudioSource source;
    public AudioClip somtiro;
    public AudioClip somtironormal;
    public AudioClip pulo;
    public AudioClip dash;
    public AudioClip run;

    void Awake()
    {
        animator = GetComponent<Animator>();
        TryGetComponent(out source);
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
            source.PlayOneShot(pulo);
        }

        if (jump)
        {
            controller.Jump();
            jump = false;
            canShoot = false; // Disable shooting while jumping
        }

        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.X) && horizontalMove == 0f && canShoot && transition == 0 && !jump)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Z) && horizontalMove == 0f && canShoot && transition == 0 && !jump)
        {
            AtaqueEspecial();
        }

        if (playerHealth <= 0)
        {
            ResetScene();
        }

        float healthPercentage = (float)playerHealth / 5f;
        healthBar.localScale = new Vector3(healthPercentage, 1f, 1f);
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            source.PlayOneShot(run);
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

        if (dashTimer > 0f)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                canDash = true;
            }
        }

        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.fixedDeltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        canShoot = true; // Enable shooting when landing
    }

    void Dash()
    {
        if (horizontalMove != 0f && SceneManager.GetActiveScene().name == "Boss2" || SceneManager.GetActiveScene().name == "Boss3")
        {
            canDash = false;
            dashTimer = dashDuration;
            source.PlayOneShot(dash);
            controller.Move(horizontalMove * dashForce * Time.fixedDeltaTime, false, false);

            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }

    void Shoot()
    {
        transition = 1;
        animator.SetInteger("transition", transition);
        source.PlayOneShot(somtironormal);
        GameObject bullet = Instantiate(bala, balaPoint.position, balaPoint.rotation);
        Bala bulletScript = bullet.GetComponent<Bala>();
        bulletScript.SetDirection(transform.localScale.x);

        canShoot = false;
        Invoke("ResetShootCooldown", shootCooldown);
    }

    void AtaqueEspecial()
    {
        if (transition == 0)
        {
            transition = 1;
            animator.SetInteger("transition", transition);
            source.PlayOneShot(somtiro);
            GameObject bullet = Instantiate(balaEspecial, balaPoint.position, balaPoint.rotation);
            Bala bulletScript = bullet.GetComponent<Bala>();
            bulletScript.SetDirection(transform.localScale.x);

            canShoot = false;
            Invoke("ResetShootCooldown", shootCooldown);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BalaPlayer")
        {
            playerHealth--;
        }
    
        if (other.gameObject.tag == "Enemy")
        {
            playerHealth -= 4;
            Vector2 knockbackDirection = transform.position - other.transform.position;
            knockbackDirection = knockbackDirection.normalized;
            float knockbackForce = 20f; // Defina a força do knockback aqui

            Vector2 knockbackVelocity = knockbackDirection * knockbackForce;
            controller.Move(knockbackVelocity.x, false, false);

            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }


    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ResetShootCooldown()
    {
        canShoot = true;
        transition = 0;
        animator.SetInteger("transition", transition);
    }
}
