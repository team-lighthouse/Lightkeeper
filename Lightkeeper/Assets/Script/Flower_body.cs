using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_body : MonoBehaviour
{
    public bool hit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed"))
        {
            hit = true;
            SoundManager.instance.soundMonsterDeath();
        }
    }
}
