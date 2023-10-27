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

    private Transform player;
    private bool isLowHealth = false;

    private void Start()
    {
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
        // Implement any animation or logic for walking
    }

    private void PerformAttack()
    {
        // Implement attack logic here
        // Apply damage to the player or trigger any other desired behavior
    }

    private void PerformSuperAttack()
    {
        // Implement super attack logic here
        // Apply higher damage to the player or trigger any other desired behavior
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
    }

    private bool IsPlayerInRange()
    {
        // Implement logic to check if the player is within attack range
        // Return true if the player is in range, false otherwise
        return false;
    }
}

