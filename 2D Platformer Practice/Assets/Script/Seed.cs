using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float bulletmovePow = 10f;
    public int bulletDirection;

    void FixedUpdate()
    {
        Vector3 moveVelocity = new Vector3(bulletDirection, 0, 0);

        transform.position += moveVelocity * bulletmovePow * Time.deltaTime;

        if (transform.position.x < -10 || transform.position.x > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enter");
        Destroy(gameObject);
        if (col.gameObject.tag == "Dirt")
        {
            //Instantiate tree platform
            Debug.Log("Dirt");
        }
    }
}
