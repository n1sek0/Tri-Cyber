using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss03 : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float lowHealthThreshold = 30f;
    public float movementSpeed = 5f;
    public float attackDamage = 10f;
    public float superAttackDamage = 20f;
    public ParticleSystem superAttackEffects;
    private Rigidbody2D rig;
    private Animator anim;
    private Transform player;
    private bool isLowHealth = false;
        

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (currentHealth <= lowHealthThreshold && !isLowHealth)
        {
            isLowHealth = true;
            PerformSuperAttack();
        }
        else if (IsPlayerInRange())
        {
            PerformAttack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        float movement = Input.GetAxis("Horizontal");
        Debug.Log(movement);
        rig.velocity = new Vector2(movement * movementSpeed, rig.velocity.y);

        if (movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void PerformAttack()
    {
        int damage = CalculateDamage();
        ApplyDamageToPlayer(damage);
    }

    private int CalculateDamage()
    {
        return 10; // Exemplo de valor, substitua pela sua própria lógica
    }

    private void ApplyDamageToPlayer(int damage)
    {
        // Implemente a lógica para aplicar o dano ao jogador
        // Você pode reduzir a saúde do jogador ou acionar outras ações com base no valor de dano calculado
        currentHealth -= damage;
    }
    
    private void ApplyDamageToPlayer(GameObject player, int damage)
    {
        // Implemente a lógica para aplicar o dano ao jogador usando o GameObject do jogador e o valor de dano fornecido
        // Você pode reduzir a saúde do jogador ou acionar outras ações com base no valor de dano calculado
        // Exemplo:
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    private void PerformSuperAttack()
    {
        GameObject target = GetTarget();

        if (target != null)
        {
            int damage = CalculateSuperAttackDamage();
            ApplyDamageToPlayer(target, damage);
            TriggerSuperAttackEffects();
        }
    }

    private int CalculateSuperAttackDamage()
    {
        return 50; // Exemplo de valor, substitua pela sua própria lógica
    }

    private void TriggerSuperAttackEffects()
    {
        // Implemente a lógica para acionar quaisquer outros efeitos desejados específicos do super ataque
        // Isso pode incluir efeitos visuais, efeitos sonoros, animações, etc.
        // Exemplo:
        superAttackEffects.Play();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implemente a lógica de morte aqui
        // Destrua o inimigo ou acione qualquer outro comportamento desejado
        // Exemplo: Destrua o objeto do inimigo
        Destroy(gameObject);
    }

    private bool IsPlayerInRange()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);
            float attackRange = 10f; // Exemplo de valor, substitua pela sua própria faixa de ataque desejada

            if (distance <= attackRange)
            {
                return true;
            }
        }

        return false;
    }

    private GameObject GetTarget()
    {
        // Implemente a lógica para obter o alvo desejado
        // Por exemplo, você pode procurar por um objeto com uma determinada tag ou usar alguma outra lógica personalizada
        // Retorne o GameObject do alvo ou null se nenhum alvo for encontrado
        return null;
    }
}