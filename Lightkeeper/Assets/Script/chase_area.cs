using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase_area : MonoBehaviour
{
    public bool playerDetect = false; // Bat.cs change this variable when player respawn
    public GameObject player; // Bat.cs use this variable (for chasing player)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerDetect = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerDetect = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // Player
        {
            playerDetect = false;
        }
    }
}
