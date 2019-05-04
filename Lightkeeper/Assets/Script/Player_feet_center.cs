using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_feet_center : MonoBehaviour
{
    public bool feetOnIce = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 22) // Ice
        {
            feetOnIce = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 22) // Ice
        {
            feetOnIce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 22) // Ice
        {
            feetOnIce = false;
        }
    }

}
