using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private float horizontalMove = 0f;
    public float runSpeed = 40f;
    private bool jump = false;
    public Animator animator;
    private bool isAtk;
    public GameObject bala;
    public Transform balaPoint;

    private bool canDash = true; // Flag to track if the player can dash
    public float dashForce = 100f; // The force applied during the dash
    public float dashDuration = 0.2f; // The duration of the dash in seconds
    private float dashTimer = 0f; // Timer to track the dash duration

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
    }

    void Dash()
    {
        if (horizontalMove != 0f)
        {
            canDash = false;
            dashTimer = dashDuration;

            // Apply a dash force in the direction of movement
            controller.Move(horizontalMove * dashForce * Time.fixedDeltaTime, false, false);
        }
    }
}


