using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_body : MonoBehaviour
{
    public bool bodyHit = false; // Player.cs change this variable (after respawn)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15) // Enemy
        {
            bodyHit = true;
        }
    }
}
