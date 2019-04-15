using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Seed : MonoBehaviour
{
    public float bulletmovePow;
    public int bulletDirection;
    public GameObject tree;
    
    GameObject player;

    void Start()
    {
        bulletmovePow = 30f;
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (col.gameObject.layer == 8 || col.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }

        /*
        if (col.CompareTag("Dirt"))
        {
            Vector3 dirtOnTrigger = gameObject.transform.position;
            Instantiate(point, dirtOnTrigger, transform.rotation);
            Debug.Log("now " + dirtOnTrigger.x);

            Vector3Int tileOnTrigger = tilemap_dirt.WorldToCell(dirtOnTrigger);

            if (!tilemap_dirt.HasTile(tileOnTrigger))
            {
                tileOnTrigger = tileOnTrigger + Vector3Int.right * bulletDirection;
            }

            Vector3 tileCenter = mapgrid.GetCellCenterWorld(tileOnTrigger);
            
            float posY = transform.position.y;

            if (!tilemap_dirt.HasTile(tileOnTrigger + Vector3Int.up) && posY > tileCenter.y + 0.25f)
            {
                Debug.Log("up");
                posY = tileCenter.y + 0.25f;
            }
            else if (!tilemap_dirt.HasTile(tileOnTrigger + Vector3Int.down) && posY > tileCenter.y + 0.25f)
            {
                Debug.Log("down");
                posY = tileCenter.y - 0.25f;
            }

            Instantiate(tree, new Vector3((tileCenter.x - 2.5f * bulletDirection), posY, 0), transform.rotation);

        }
        */
    }
}
