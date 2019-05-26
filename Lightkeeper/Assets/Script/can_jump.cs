using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class can_jump : MonoBehaviour
{
    new SpriteRenderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if ((gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "rise" ||
            gameObject.GetComponentInParent<Player>().animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "fall") &&
            gameObject.GetComponentInParent<Player>().canJumpNum > 0)
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }
}
