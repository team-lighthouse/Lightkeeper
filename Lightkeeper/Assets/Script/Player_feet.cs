using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_feet : MonoBehaviour
{
    public bool feetOnPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            feetOnPlatform = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            feetOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            feetOnPlatform = false;
            gameObject.GetComponentInParent<Player>().canJumpNum--;
        }
    }

}
