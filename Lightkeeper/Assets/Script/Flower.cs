using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    bool live;
    public GameObject bullet;
    float bulletTimer = 0;
    public float bulletDelay = 1f;
    public float bulletStart = 0;
    GameObject player;
    float regenTimer = 0;
    public int direction; // 1: up, 2: down, 3: left, 4: right
    new SpriteRenderer renderer;
    BoxCollider2D bc;

    void Start()
    {
        live = true;
        player = GameObject.FindGameObjectWithTag("Player");
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
                switch (direction)
                {
                    case 1: // up
                        newBullet.GetComponent<Bullet>().direction = new Vector2(0, 12f);
                        break;
                    case 2: // down
                        newBullet.GetComponent<Bullet>().direction = new Vector2(0, -12f);
                        break;
                    case 3: // left
                        newBullet.GetComponent<Bullet>().direction = new Vector2(-12f, 0);
                        break;
                    case 4: // right
                        newBullet.GetComponent<Bullet>().direction = new Vector2(12f, 0);
                        break;
                    default: // will not run
                        newBullet.GetComponent<Bullet>().direction = new Vector2(0, 12f);
                        break;
                }
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
            bulletTimer = bulletStart;
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
