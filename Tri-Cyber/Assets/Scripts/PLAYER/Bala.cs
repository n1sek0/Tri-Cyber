using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D rig;
    public float speed;
    private float direction = 1f; // Default direction is right
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = Vector2.right * speed * direction;

        // Flip the sprite based on the direction
        if (direction < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction > 0f)
        {
            spriteRenderer.flipX = false;
        }
    }

    // Method to set the direction of the bullet
    public void SetDirection(float newDirection)
    {
        direction = newDirection;
    }
}