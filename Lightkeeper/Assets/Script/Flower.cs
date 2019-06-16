using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    bool live;
    public GameObject bullet;
    float bulletTimer = 0;
    public float bulletDelay = 1f;
    GameObject player;
    float regenTimer = 0;
    Vector2 direction;
    new SpriteRenderer renderer;
    BoxCollider2D bc;

    void Start()
    {
        live = true;
        player = GameObject.FindGameObjectWithTag("Player");
        direction = GetComponentInChildren<Transform>().transform.position - transform.position;
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!live) // flower is dead
        {
            // do nothing
        }
        else // flower is live
        {
            if (bulletTimer >= bulletDelay)
            {
                bulletTimer = 0;
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                newBullet.GetComponent<Bullet>().direction = this.direction;
            }
            else
            {
                bulletTimer += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (gameObject.GetComponentInChildren<Flower_body>().hit) // hit by seed
        {
            live = false;
            renderer.enabled = false;
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        }

        if (regenTimer >= 2f && player.GetComponent<Player>().live) // Player respawned
        {
            live = true;
            renderer.enabled = true;
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<Flower_body>().hit = false;
            regenTimer = 0;
        }

        if (!player.GetComponent<Player>().live) // player is dead
        {
            regenTimer += Time.deltaTime;
        }
    }
}
