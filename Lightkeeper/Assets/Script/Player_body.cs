using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_body : MonoBehaviour
{
    public bool bodyHit = false; // Player.cs change this variable (after respawn)
    bool bodyHitCount = false;
    float bodyHitTimer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15 || // Enemy
            collision.gameObject.layer == 17) // Thorn
        {
            bodyHitCount = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15 || // Enemy
            collision.gameObject.layer == 17) // Thorn
        {
            bodyHitTimer += Time.deltaTime;
        }
        if (bodyHitTimer > 0.03f)
        {
            bodyHit = true;
            bodyHitCount = false;
            bodyHitTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15 || // Enemy
            collision.gameObject.layer == 17) // Thorn
        {
            bodyHitCount = false;
            bodyHitTimer = 0;
        }
    }
}
