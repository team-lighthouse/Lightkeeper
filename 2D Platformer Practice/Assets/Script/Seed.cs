using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float bulletmovePow = 40f;
    public int bulletDirection;
    public GameObject tree;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        Vector3 moveVelocity = new Vector3(bulletDirection, 0, 0);

        transform.position += moveVelocity * bulletmovePow * Time.deltaTime;

        if (transform.position.x < player.transform.position.x-25 || transform.position.x > player.transform.position.x + 25)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        if (col.gameObject.tag == "Dirt")
        {
            if (bulletDirection < 0)
            {
                Instantiate(tree,  new Vector3((col.gameObject.transform.position.x + 1.5f), transform.position.y, 0), transform.rotation);
            }
            else // bulletDirection >= 0
            {
                Instantiate(tree, new Vector3((col.gameObject.transform.position.x - 1.5f), transform.position.y, 0), transform.rotation);
            }
        }
        Destroy(gameObject);
    }
}
