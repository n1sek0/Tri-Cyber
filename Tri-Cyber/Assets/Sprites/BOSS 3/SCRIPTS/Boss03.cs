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
        transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);

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
        return 10; // Example value, replace with your own logic
    }

    private void ApplyDamageToPlayer(int damage)
    {
        // Implement the logic to apply the damage to the player
        // You can reduce the player's health or trigger other actions based on the calculated damage value
        currentHealth -= damage;
    }

    private void PerformSuperAttack()
    {
        GameObject target = GetTarget();

        if (target != null)
        {
            int damage = CalculateSuperAttackDamage();
            ApplyDamage(target, damage);
            TriggerSuperAttackEffects();
        }
    }

    private int CalculateSuperAttackDamage()
    {
        return 50; // Example value, replace with your own logic
    }

    private void TriggerSuperAttackEffects()
    {
        // Implement the logic to trigger any other desired effects specific to the super attack
        // This can include visual effects, sound effects, animations, etc.
        // Example:
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
        // Implement death logic here
        // Destroy the enemy or trigger any other desired behavior
        // Example: Destroy the enemy game object
        Destroy(gameObject);
    }

    private bool IsPlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float attackRange = 10f; // Example value, replace with your desired attack range

            if (distance <= attackRange)
            {
                return true;
            }
        }

        return false;
    }
}
