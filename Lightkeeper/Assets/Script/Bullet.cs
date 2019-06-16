using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    Rigidbody2D rb;
    GameObject player;
    float regenTimer = 0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        
        if (regenTimer >= 2f) // Player respawned, destroy this bullet
        {
            Destroy(gameObject);
        }

        if (!player.GetComponent<Player>().live) // Player is dead
        {
            rb.velocity = Vector2.zero;
            regenTimer += Time.deltaTime;
        }
        else
        {
            rb.velocity = direction;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Hard_platform
            collision.gameObject.layer == 21) // Soft_platform
        {
            Destroy(gameObject);
        }
    }
}
