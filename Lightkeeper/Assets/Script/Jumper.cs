using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Sprite jumper01;
    public Sprite jumper02;

    float regenTimer;
    float jumperRegenTime = 0.3f;

    new SpriteRenderer renderer;
    BoxCollider2D bc;

    void Start()
    {
        regenTimer = jumperRegenTime;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (regenTimer < jumperRegenTime)
        {
            regenTimer += Time.deltaTime;
        }
        else
        {
            renderer.sprite = jumper01;
            bc.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_feet"))
        {
            renderer.sprite = jumper02;
            bc.enabled = false;
            regenTimer = 0f;
        }
    }


}
