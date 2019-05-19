using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_patrol : MonoBehaviour
{
    public float patrolTime;
    float timeCount = 0;
    Vector3 startPos;
    Vector2 velocity;
    int direction = 1;
    float regenTimer = 0; // in Player.cs 'deadTime' is over

    GameObject player;
    Rigidbody2D rb;
    Collider2D col;
    Animator animator;
    new SpriteRenderer renderer;
    
    void Start()
    {
        startPos = transform.position;
        velocity = gameObject.GetComponentInChildren<patrol_end>().endPosition - startPos;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();
        animator = gameObject.GetComponent<Animator>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            if (regenTimer >= 3f)
            {
                renderer.enabled = true;
                col.enabled = true;
                transform.position = startPos;
                timeCount = 0;
                direction = 1;
                regenTimer = 0;
            }

            if (!player.GetComponent<Player>().live)
            {
                rb.velocity = Vector2.zero;
                animator.enabled = false;
                regenTimer += Time.deltaTime;
            }
            else
            {
                animator.enabled = true;
                timeCount += Time.deltaTime;
                if (timeCount > patrolTime)
                {
                    direction = -direction;
                    timeCount = 0;
                }
                rb.velocity = velocity * direction / patrolTime;

                if (rb.velocity.x >= 0)
                {
                    renderer.flipX = true;
                }
                else
                {
                    renderer.flipX = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed"))
        {
            renderer.enabled = false;
            col.enabled = false;
        }
    }
}
