using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Sprite pusher01;
    public Sprite pusher02;

    float regenTimer;
    float pusherRegenTime = 0.3f;

    new SpriteRenderer renderer;
    BoxCollider2D bc;

    void Start()
    {
        regenTimer = pusherRegenTime;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (regenTimer < pusherRegenTime)
        {
            regenTimer += Time.deltaTime;
        }
        else
        {
            renderer.sprite = pusher01;
            bc.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            renderer.sprite = pusher02;
            bc.enabled = false;
            regenTimer = 0f;
        }
    }
}
