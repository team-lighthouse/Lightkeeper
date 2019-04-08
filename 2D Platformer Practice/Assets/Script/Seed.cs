using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Seed : MonoBehaviour
{
    public float bulletmovePow = 30f;
    public int bulletDirection;
    public GameObject tree;
    public Tilemap tilemap_dirt;

    GameObject player;
    Grid mapgrid;

    void Start()
    {
        player = GameObject.Find("Player");
        tilemap_dirt = GameObject.Find("wall_dirt").GetComponent<Tilemap>();
        mapgrid = GameObject.Find("MapGrid").GetComponent<Grid>();
    }

    void FixedUpdate()
    {
        Vector3 moveVelocity = new Vector3(bulletDirection, 0, 0);

        transform.position += moveVelocity * bulletmovePow * Time.deltaTime;

        if (transform.position.x < player.transform.position.x-25 ||
            transform.position.x > player.transform.position.x+25)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        /*
        if (col.gameObject.tag == "Dirt")
        {
            if (bulletDirection < 0)
            {
                Instantiate(tree, new Vector3((col.gameObject.transform.position.x + 1.5f), transform.position.y, 0), transform.rotation);
            }
            else // bulletDirection >= 0
            {
                Instantiate(tree, new Vector3((col.gameObject.transform.position.x - 1.5f), transform.position.y, 0), transform.rotation);
            }
        }
        */
        Vector3 triggerAdjust = Vector3.right * 0.25f;

        if (bulletDirection < 0)
        {
             triggerAdjust = Vector3.left * 0.25f;
        }

        Vector3 dirtOnTrigger = gameObject.transform.position + triggerAdjust;

        Vector3Int tileOnTrigger = tilemap_dirt.WorldToCell(dirtOnTrigger);
        Vector3 tileCenter = mapgrid.GetCellCenterWorld(tileOnTrigger);
        
        if (bulletDirection < 0)
        {
            Instantiate(tree, new Vector3((tileCenter.x + 1.5f), transform.position.y, 0), transform.rotation);
        }
        if (bulletDirection >= 0)
        {
            Instantiate(tree, new Vector3((tileCenter.x - 1.5f), transform.position.y, 0), transform.rotation);
        }
        
        Destroy(gameObject);
    }
}
