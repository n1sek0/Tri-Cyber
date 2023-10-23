using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public float speed;
    public float timer;
    public float walktime;
    static int vidaCururu;
    public int damage = 1;

    public bool walkRight = true;
    
    private Rigidbody2D rig;
    private Animator anim;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer >= walktime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }
        if (walkRight)
        {
            transform.eulerAngles = new Vector2(0,0);
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector2(0,180);
            rig.velocity = Vector2.left * speed;
        }
    }

    private void Update()
    {
        atack();
    }

    void atack()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            anim.SetInteger("trasition",1);
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //col.gameObject.GetComponent<Player>().damage(damage);
        }
    }
    
    
}
