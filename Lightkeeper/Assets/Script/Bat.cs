using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float speed;
    float timeCount = 0;
    Vector3 startPos;
    bool moveFromStart = true;
    float regenTimer = 0;
    Vector3 curPos;
    bool chase = false;

    GameObject player;
    Rigidbody2D rb;
    Animator animator;
    new SpriteRenderer renderer;
    
    void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (regenTimer >= 2f && player.GetComponent<Player>().live) // Player respawned
        {
            renderer.enabled = true;
            rb.WakeUp();
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            transform.position = startPos;
            timeCount = 0;
            moveFromStart = true;
            regenTimer = 0;
            gameObject.GetComponentInChildren<chase_area>().playerDetect = false;
            gameObject.GetComponentInChildren<Bat_body>().hit = false;
        }
        
        if (gameObject.GetComponentInChildren<Bat_body>().hit) // hit by seed
        {
            renderer.enabled = false;
            rb.Sleep();
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        }

        if (!player.GetComponent<Player>().live) // Player is dead
        {
            rb.velocity = Vector2.zero;
            animator.enabled = false;
            regenTimer += Time.deltaTime;
        }
        else // Player is alive
        {
            animator.enabled = true;

            if (!chase)
            {
                curPos = transform.position;

                if (moveFromStart)
                {
                    rb.velocity = Vector3.Normalize(gameObject.GetComponentInChildren<patrol_point>().endPosition - transform.position) * speed;
                    if (gameObject.GetComponentInChildren<BoxCollider2D>().bounds.Contains(gameObject.GetComponentInChildren<patrol_point>().endPosition))
                    {
                        moveFromStart = false;
                    }
                }
                else
                {
                    rb.velocity = Vector3.Normalize(startPos - transform.position) * speed;
                    if (gameObject.GetComponentInChildren<BoxCollider2D>().bounds.Contains(startPos))
                    {
                        moveFromStart = true;
                    }
                }
                
                chase = gameObject.GetComponentInChildren<chase_area>().playerDetect;
                Debug.Log(moveFromStart);
            }
            else // chase
            {
                if (chase == gameObject.GetComponentInChildren<chase_area>().playerDetect) // Player is near
                {
                    rb.velocity = Vector3.Normalize(gameObject.GetComponentInChildren<chase_area>().player.transform.position - transform.position) * speed;
                }
                else // Player is gone
                {
                    rb.velocity = Vector3.Normalize(curPos - transform.position) * speed;
                    if (gameObject.GetComponentInChildren<BoxCollider2D>().bounds.Contains(curPos))
                    {
                        chase = false;
                    }
                }
            }
            
            if (rb.velocity.x >= 0)
            {
                renderer.flipX = true;
                gameObject.GetComponentInChildren<CapsuleCollider2D>().offset = new Vector2(0.3f, -0.05f);
            }
            else
            {
                renderer.flipX = false;
                gameObject.GetComponentInChildren<CapsuleCollider2D>().offset = new Vector2(-0.3f, -0.05f);
            }
        }
    }
}
