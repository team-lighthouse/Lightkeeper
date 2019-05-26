using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_jump : MonoBehaviour
{
    float regenTimer;
    float itemRegenTime = 3f;
    
    new SpriteRenderer renderer;
    BoxCollider2D bc;

    void Start()
    {
        regenTimer = itemRegenTime;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        if (regenTimer < itemRegenTime)
        {
            regenTimer += Time.deltaTime;
        }
        else
        {
            renderer.enabled = true;
            bc.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            renderer.enabled = false;
            bc.enabled = false;
            regenTimer = 0f;
            collision.gameObject.GetComponent<Player>().canJumpNum++;
        }
    }
}
