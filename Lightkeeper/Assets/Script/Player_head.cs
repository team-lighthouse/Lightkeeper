using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_head : MonoBehaviour
{
    public bool headTouch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Hard_platform
        {
            headTouch = true;
            gameObject.GetComponentInParent<Player>().isJumping = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Hard_platform
        {
            headTouch = true;
            gameObject.GetComponentInParent<Player>().isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Hard_platform
        {
            headTouch = false;
        }
    }
}
