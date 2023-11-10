using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Float:")]
    public float vida = 40f;
    public float velocidade = 2;
    public float velocidadeEstagio2 = 6f;
    public float tempoEntreTiros = 2f;
    public float patrolDistance = 5.0f;
    public float attackDistance = 10.0f;
    public float speedBuleet = 11;

    [Header("Bool:")]
    public bool estaMorto = false;
    private bool movingRight = true;
    private bool isShooting = false;
    private bool isStopped = false;

    [Header("Componentes")]
    public Rigidbody2D rig;
    public Animator anim;
    public GameObject bala;
    public GameObject balaespecial;
    public Transform pontoDeTiro;
    public Transform jogador;
    public AudioSource source;

    private int estagioAtual = 1;
    private Vector3 startPosition;
    private enum State
    {
        Patrol,
        Attack,
        Dead
    }
    private State currentState = State.Patrol;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        estagioAtual = 1;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (estaMorto)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, jogador.position);
        bool jogadorTaNaDireita =  transform.position.x  < jogador.position.x;

        switch (currentState)
        {
            case State.Patrol:
                MoverPatrulha(distanceToPlayer, jogadorTaNaDireita);
                break;

            case State.Attack:
                Atacar(distanceToPlayer, jogadorTaNaDireita);
                break;

            case State.Dead:
                Morrer();
                break;
        }

        if (estaMorto)
        {
            Verify();
        }
    }

    void Verify()
    {
        if (vida <= 0)
        {
            estaMorto = true;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 1f);
        }
    }

    private void MoverPatrulha(float distanceToPlayer, bool jogadorNaDireita)
    {
        if (distanceToPlayer <= attackDistance && movingRight == jogadorNaDireita)
        {
            isStopped = true;
            currentState = State.Attack;
        }
        else
        {
            isStopped = false;

            if (estagioAtual == 1)
            {
                MoverEstagio1();
            }
            else if (estagioAtual == 2)
            {
                MoverEstagio2();
            }
        }
    }

    private void Atacar(float distanceToPlayer, bool direcaoParaJogador)
    {
        isStopped = true;

        if (distanceToPlayer > attackDistance || movingRight != direcaoParaJogador)
        {
            isShooting = false;
            isStopped = false;
            currentState = State.Patrol;
        }

        if (!isShooting)
        {
            StartCoroutine(AtirarCoroutine());
        }
    }

    private void Morrer()
    {
        // Lógica de morte aqui
    }

    private bool PodeAtirar()
    {
        if (estaMorto)
        {
            return false;
        }

        return true;
    }

    private IEnumerator AtirarCoroutine()
    {
        if (vida >21)
        {
            isShooting = true;
            anim.SetBool("attack", true);
            yield return new WaitForSecondsRealtime(0.5f);
            isShooting = false;
            source.Play();
            GameObject bullet = Instantiate(bala, pontoDeTiro.position, pontoDeTiro.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(speedBuleet, 0);
            Destroy(bullet, 2f);
            anim.SetBool("attack", false);
            yield return new WaitForSecondsRealtime(3f);
        }

        if (vida <=20)
        {
            isShooting = true;
            anim.SetBool("attack", true);
            yield return new WaitForSecondsRealtime(0.5f);
            isShooting = false;
            source.Play();
            GameObject bullet = Instantiate(balaespecial, pontoDeTiro.position, pontoDeTiro.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(speedBuleet, 0);
            Destroy(bullet, 2f);
            anim.SetBool("attack", false);
            yield return new WaitForSecondsRealtime(3f);
        }
    }

    private void MoverEstagio1()
    {
        if (movingRight)
        {
            speedBuleet = 15;
            rig.velocity = new Vector2(velocidade, rig.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 0.5159f, 0.5159f);
        }
        else
        {
            speedBuleet = -15;
            rig.velocity = new Vector2(-velocidade, rig.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 0.5159f, 0.5159f);
        }

        if (transform.position.x > startPosition.x + patrolDistance && movingRight)
        {
            movingRight = false;
        }
        else if (transform.position.x < startPosition.x - patrolDistance && !movingRight)
        {
            movingRight = true;
        }
    }

    private void MoverEstagio2()
    {
        if (movingRight)
        {
            rig.velocity = new Vector2(velocidadeEstagio2, rig.velocity.y);
            transform.localScale =  new Vector3(Mathf.Abs(transform.localScale.x), 0.5159f, 0.5159f);
        }
        else
        {
            rig.velocity = new Vector2(-velocidadeEstagio2, rig.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 0.5159f, 0.5159f);
        }

        if (transform.position.x > startPosition.x + patrolDistance && movingRight)
        {
            movingRight = false;
        }
        else if (transform.position.x < startPosition.x - patrolDistance && !movingRight)
        {
            movingRight = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BalaEnemy")
        {
            vida--;
            
        }
    }
    

    // Função para controlar quando o inimigo está parado
    public void SetStopped(bool stopped)
    {
        isStopped = stopped;
    }

    // Função para verificar se o inimigo está virado de frente para o jogador
    private bool EstaViradoParaJogador(Vector3 direcaoParaJogador)
    {
        Vector3 direcaoInimigo = transform.right;
        float angulo = Vector3.Angle(direcaoInimigo, direcaoParaJogador);
        return angulo < 35.0f; // Defina o ângulo desejado para "frente" aqui
    }
}

