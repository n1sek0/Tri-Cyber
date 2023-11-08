using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2 : MonoBehaviour
{
    [Header("Float:")]
    public float vida = 40f;
    public float velocidade = 2;
    public float velocidadeEstagio2 = 7f;
    public float taxaDeTiro = 5f;
    public float tempoDesdeUltimoTiro = 0;
    public float tempoEntreTiros = 2f;
    public float patrolDistance = 5.0f;
    public float speedBuleet = 11;

    [Header("Bool:")]
    public bool estaMorto = false;
    private bool movingRight = true;

    [Header("Componentes")]
    public Rigidbody2D rig;
    public Animator anim;
    public GameObject bala;
    public Transform pontoDeTiro;
    public Transform jogador;
    public AudioClip[] audios;
    public AudioSource source;

    private int estagioAtual = 1;
    private bool isShooting = false;
    private Vector3 startPosition; 

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

        if (estagioAtual == 1)
        {
            MoverEstagio1();
        }
        else if (estagioAtual == 2)
        {
            MoverEstagio2();
        }
        if (PodeAtirar())
        {
            Vector3 direcaoParaJogador = jogador.position - transform.position;
            float anguloParaJogador = Vector3.Angle(Vector3.right, direcaoParaJogador);

            if (Mathf.Abs(anguloParaJogador) < 30f || Math.Abs(anguloParaJogador) > 30f)
            {
                if (!isShooting)
                {
                    StartCoroutine(AtirarCoroutine());
                }
            }
        }

        if (estaMorto == false)
        {
            Verify();
        }
    }
    void Verify()
    {
        if (vida <= 0)
        {
            estaMorto = true;
            playAudio(0);
            anim.SetTrigger("Dead");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Invoke("playAudio", 2.8f);
            Destroy(gameObject, 3f);
        }
        if (vida <= 20)
        {
            estagioAtual = 2;
            anim.SetInteger("Transition", 1);
        }
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
        isShooting = true;
        anim.SetBool("IsFire", true);
        yield return new WaitForSeconds(1f);
        if (estaMorto)
        {
            isShooting = false;
            yield break;
        }
        playAudio(1);
        GameObject bullet = Instantiate(bala, pontoDeTiro.position, pontoDeTiro.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speedBuleet, 0);
        Destroy(bullet, 2f);
        anim.SetBool("IsFire", false);
        yield return new WaitForSeconds(1f); 
        isShooting = false;
    }

    private void MoverEstagio1()
    {
        if (movingRight)
        {
            speedBuleet = 15;
            rig.velocity = new Vector2(velocidade, rig.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 1.77636f, 1.77636f);
        }
        else
        {
            speedBuleet = -15;
            rig.velocity = new Vector2(-velocidade, rig.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 1.77636f, 1.77636f);
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
            speedBuleet = 15;
            rig.velocity = new Vector2(velocidadeEstagio2, rig.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 1.77636f, 1.77636f);
        }
        else
        {
            speedBuleet = -15;
            rig.velocity = new Vector2(-velocidadeEstagio2, rig.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 1.77636f, 1.77636f);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BalaEnemy")
        {
            vida--;
        }
    }
    void playAudio(int valor)
    {
        if (valor >= 0 && valor < audios.Length)
        {
            source.clip = audios[valor];
            source.Play();
        }
    }
}
