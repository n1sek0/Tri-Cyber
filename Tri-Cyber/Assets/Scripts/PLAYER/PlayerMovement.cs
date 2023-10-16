using UnityEngine;

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

    private bool canDash = true; // Flag to track if the player can dash
    public float dashForce = 100f; // The force applied during the dash
    public float dashDuration = 0.2f; // The duration of the dash in seconds
    private float dashTimer = 0f; // Timer to track the dash duration

    private bool isInvulnerable = false; // Flag to track if the player is invulnerable
    public float invulnerabilityDuration = 1f; // The duration of invulnerability in seconds
    private float invulnerabilityTimer = 0f; // Timer to track the invulnerability duration

    private bool canShoot = true; // Flag to track if the player can shoot
    public float shootCooldown = 1f; // The cooldown between shots in seconds

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.X) && horizontalMove == 0f && canShoot && transition == 0)
        {
            Shoot();
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

    void Dash()
    {
        if (horizontalMove != 0f)
        {
            canDash = false;
            dashTimer = dashDuration;

            controller.Move(horizontalMove * dashForce * Time.fixedDeltaTime, false, false);

            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }

    void Shoot()
    {
        transition = 1;
        animator.SetInteger("transition", transition);

        GameObject bullet = Instantiate(bala, balaPoint.position, balaPoint.rotation);
        Bala bulletScript = bullet.GetComponent<Bala>();
        bulletScript.SetDirection(transform.localScale.x);

        canShoot = false;
        Invoke("ResetShootCooldown", shootCooldown);
    }
    
    void AtaqueEspecial()
    {
        animator.SetInteger("transition", 1);

        GameObject bullet = Instantiate(balaEspecial, balaPoint.position, balaPoint.rotation);
        Bala bulletScript = bullet.GetComponent<Bala>();
        bulletScript.SetDirection(transform.localScale.x);

        canShoot = false;
        Invoke("ResetShootCooldown", shootCooldown);
    }


    void ResetShootCooldown()
    {
        canShoot = true;
        transition = 0;
        animator.SetInteger("transition", transition);
    }
}
