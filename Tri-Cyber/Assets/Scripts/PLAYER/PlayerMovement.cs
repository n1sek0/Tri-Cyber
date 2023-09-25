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
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
        }

        // Verifica se a animação de ataque terminou para voltar à animação de idle
        if (isAtk && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            isAtk = false;
            animator.SetInteger("transition", 0);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    void Shoot()
    {
        if (!isAtk)
        {
            isAtk = true;
            animator.SetInteger("transition", 1);

            // Verificar a direção do jogador para definir a rotação da bala
            Vector3 balaRotation = Quaternion.Euler(0, 0, controller.FacingRight ? 0f : 180f) * balaPoint.rotation.eulerAngles;
            Instantiate(bala, balaPoint.position, Quaternion.Euler(balaRotation));
        }
    }
}
