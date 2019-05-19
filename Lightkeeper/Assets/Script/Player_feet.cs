using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_feet : MonoBehaviour
{
    public bool feetOnPlatform;
    public bool feetOnJumper;
    public bool feetOnIce;
    bool iceTimerRun = false;
    float iceTimer;
    float iceTimerDuration = 0.2f;

    private void FixedUpdate()
    {
        if (iceTimerRun)
        {
            iceTimer += Time.deltaTime;

            if (iceTimer >= iceTimerDuration)
            {
                feetOnIce = false;
                iceTimerRun = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jumper"))
        {
            feetOnJumper = true;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            feetOnIce = true;
            iceTimerRun = false;
        }
        if (collision.gameObject.layer == 20 || // Hard_platform
            collision.gameObject.layer == 21) // Soft_platform
        {
            feetOnPlatform = true;
            if (!collision.gameObject.CompareTag("Ice") &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "rise" &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "fall")
            {
                feetOnIce = false;
                iceTimerRun = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            feetOnIce = true;
            iceTimerRun = false;
        }
        if (collision.gameObject.layer == 20 || // Hard_platform
            collision.gameObject.layer == 21) // Soft_platform
        {
            feetOnPlatform = true;
            if (!collision.gameObject.CompareTag("Ice") &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "rise" &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "fall")
            {
                feetOnIce = false;
                iceTimerRun = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            iceTimerRun = true;
            iceTimer = 0;
        }
        if (collision.gameObject.layer == 20 || // Hard_platform
           collision.gameObject.layer == 21) // Soft_platform
        {
            if (!feetOnJumper &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "rise" &&
                gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "fall")
            {
                gameObject.GetComponentInParent<Player>().canJumpNum--;
            }
            feetOnPlatform = false;
        }
    }
}
