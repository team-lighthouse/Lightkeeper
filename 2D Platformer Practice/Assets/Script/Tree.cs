using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Collider2D playerCol;

    void Start()
    {
        GameObject.Find("TreeCounter").GetComponent<TreeCounter>().trees.Enqueue(gameObject);
        playerCol = GameObject.Find("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), true);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 10 && (col.transform.position.y < transform.position.y + 1))
        {
            Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), true);
        }
        else
        {
            Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), false);

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 10)
        {
            Physics2D.IgnoreCollision(playerCol, GetComponent<Collider2D>(), false);
        }
    }
}
