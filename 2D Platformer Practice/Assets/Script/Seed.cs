using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Seed : MonoBehaviour
{
    public float bulletmovePow;
    public int bulletDirection;
    public GameObject tree;

    Tilemap tilemap_dirt;
    GameObject player;
    Grid mapgrid;

    void Start()
    {
        bulletmovePow = 30f;
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

        if (col.CompareTag("Dirt"))
        {
            Vector3 dirtOnTrigger = gameObject.transform.position;

            Vector3Int tileOnTrigger = tilemap_dirt.WorldToCell(dirtOnTrigger);

            if (!tilemap_dirt.HasTile(tileOnTrigger))
            {
                tileOnTrigger = tileOnTrigger + Vector3Int.right * bulletDirection;
                Debug.Log("dl");
            }
            else if (tilemap_dirt.HasTile(tileOnTrigger + Vector3Int.left * bulletDirection))
            {
                tileOnTrigger = tileOnTrigger + Vector3Int.left * bulletDirection;
                Debug.Log("dd");
            }

            Vector3 tileCenter = mapgrid.GetCellCenterWorld(tileOnTrigger);

            Instantiate(tree, new Vector3((tileCenter.x - 2.5f * bulletDirection), transform.position.y, 0), transform.rotation);

        }
        
        Destroy(gameObject);
    }
}
