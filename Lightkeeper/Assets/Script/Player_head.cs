using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_head : MonoBehaviour
{
    public bool headTouch;

    void Update()
    {
        //Debug.Log(headTouch);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Dirt,Stone
        {
            headTouch = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Dirt,Stone
        {
            headTouch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20) // Dirt,Stone
        {
            headTouch = false;
        }
    }
}
