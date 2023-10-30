  using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2 : MonoBehaviour
{
    [Header("Float:")]
    public float life = 40f;
    public float currentHealth;
    public float speed = 2;
    public float timer;
    public float walktime;
    public float fireRate = 5f;
    public float timeSinceLastShot = 5f;
    
    [Header("Bool:")]
    public bool stage1 = true;
    public bool stage2;
    public bool walkRight = true;
    public bool Isdead;
    public bool isfire;
    
    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    public GameObject bullet;
    public GameObject bullet2;
    public Transform firePoint;
    public Transform player;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }
    void Update()
    {
        if (Isdead == false)
        {
            OlharPara();
            dead();
        }
    }

    private void FixedUpdate()
    {
        if (Isdead == false)
        {
            move();
        }
    }

    void move()
    {
        if (stage1 == true)
        {
            timer += Time.deltaTime;
        
            if (timer >= walktime)
            {
                walkRight = !walkRight;
                timer = 0f;
            }
            if (walkRight)
            {
                if (isfire == false)
                {
                    anim.SetInteger("Transition", 1);
                }
                transform.eulerAngles = new Vector2(0,0);
                rig.velocity = Vector2.right * speed;
            }

            if (!walkRight)
            {
                if (isfire == false)
                {
                    anim.SetInteger("Transition", 1);
                }
                transform.eulerAngles = new Vector2(0,180);
                rig.velocity = Vector2.left * speed;
            }
        }

        if (stage2 == true)
        {
            speed = 10f;
            timer = 0f;
            walktime = 0f;
            walkRight = true;
            if (isfire == false)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector2(0,180);
            rig.velocity = Vector2.right * speed;

        }
    }
    void dead()
    {
        if (life <= 0)
        {
            Isdead = true;
            anim.SetTrigger("Dead");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 5f);
            
        }
    }
    void OlharPara()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found.");
            return;
        }
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        if (stage1 == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        
        timeSinceLastShot -= Time.deltaTime;
        if (timeSinceLastShot <=0 && stage1 == true)
        {
            // colocar o ataque do primeiro stagio 
            timeSinceLastShot = 4f;
        }
        if (timeSinceLastShot <=0 && stage2 == true)
        {
            // colocar o ataque do segundo stagio
            timeSinceLastShot = 5f;
        }

        if (life <= 20)
        {
            stage1 = false;
            stage2 = true;
        }
    }
}
