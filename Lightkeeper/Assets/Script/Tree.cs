using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Collider2D playerCol;
    bool playerOverlap = false;
    bool ignoreCollision = true;
    float timer = 0;

    void Start()
    {
        GameObject.Find("Managers").GetComponent<InStageManager>().trees.Enqueue(gameObject);
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), true);
    }
    
    void Update()
    {
        if (timer < 0.04f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (ignoreCollision && !playerOverlap)
            {
                Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), false);
                ignoreCollision = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerOverlap = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerOverlap = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerOverlap = false;
        }
    }
}
