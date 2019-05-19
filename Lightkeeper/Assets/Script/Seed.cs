using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    float seedMoveSpeed = 30f;
    public int seedDirection; // Player.cs set this variable & Dirt.cs get this variable

    GameObject player;

    Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(seedDirection * seedMoveSpeed, 0);

        if (transform.position.x < player.transform.position.x - 40f || // out of screen
            transform.position.x > player.transform.position.x + 40f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15 || // Enemy
            collision.gameObject.layer == 20 || // Hard_platform
            collision.CompareTag("Tree"))
        {
            Destroy(gameObject);
        }
    }
}
