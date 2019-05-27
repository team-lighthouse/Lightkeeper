using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float speed;
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
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<PolygonCollider2D>().enabled = true;
            transform.position = startPos;
            moveFromStart = true;
            regenTimer = 0;
            gameObject.GetComponentInChildren<chase_area>().playerDetect = false;
            gameObject.GetComponentInChildren<Bat_body>().hit = false;
        }
        
        if (gameObject.GetComponentInChildren<Bat_body>().hit) // hit by seed
        {
            renderer.enabled = false;
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<PolygonCollider2D>().enabled = false;
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
                        if (Vector2.Distance(startPos, gameObject.GetComponentInChildren<patrol_point>().endPosition) < Vector2.kEpsilon)
                        {
                            transform.position = startPos;
                            moveFromStart = true;
                        }
                        else
                        {
                            moveFromStart = false;
                        }
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


            if (Vector2.Distance(startPos, gameObject.GetComponentInChildren<patrol_point>().endPosition) < Vector2.kEpsilon &&
                gameObject.GetComponentInChildren<BoxCollider2D>().bounds.Contains(startPos)) // not patrol, stay position
            {
                // do nothing
            }
            else
            {
                if (rb.velocity.x >= 0)
                {
                    renderer.flipX = true;
                    gameObject.GetComponentInChildren<CapsuleCollider2D>().offset = new Vector2(0.3f, -0.05f);
                    gameObject.GetComponentInChildren<PolygonCollider2D>().offset = new Vector2(0.35f, 0);
                }
                else
                {
                    renderer.flipX = false;
                    gameObject.GetComponentInChildren<CapsuleCollider2D>().offset = new Vector2(-0.3f, -0.05f);
                    gameObject.GetComponentInChildren<PolygonCollider2D>().offset = new Vector2(-0.35f, 0);
                }
            }
        }
    }
}
