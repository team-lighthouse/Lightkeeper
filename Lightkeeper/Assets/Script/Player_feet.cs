using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_feet : MonoBehaviour
{
    public bool feetOnPlatform;
    public bool feetOnJumper;
    public bool feetOnIce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            feetOnPlatform = true;
        }
        if (collision.gameObject.CompareTag("Jumper"))
        {
            feetOnJumper = true;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            feetOnIce = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            feetOnPlatform = true;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            feetOnIce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20 || // Dirt,Stone
            collision.gameObject.layer == 21) // Wood
        {
            if (!feetOnJumper && 
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "rise" &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "fall")
            {
                gameObject.GetComponentInParent<Player>().canJumpNum--;
            }
            feetOnPlatform = false;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            feetOnIce = false;
        }
    }
}
