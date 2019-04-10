using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dirt : MonoBehaviour
{
    public GameObject tree;

    Vector3 posOnTriggerBySeed;
    Vector3Int tileOnTrigger;
    int seedDirection;
    float seedPosY;

    void Start()
    {
        
    }
    
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Seed"))
        {
            seedDirection = col.GetComponent<Seed>().bulletDirection;
            seedPosY = col.transform.position.y;

            posOnTriggerBySeed = col.transform.position;

            tileOnTrigger = gameObject.GetComponent<Tilemap>().WorldToCell(posOnTriggerBySeed);

            while(!gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger))
            {
                if (gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger + Vector3Int.right * seedDirection))
                {
                    tileOnTrigger = tileOnTrigger + Vector3Int.right * seedDirection;
                }
                else
                {
                    if (!gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger + Vector3Int.up) &&
                        !gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger + Vector3Int.down))
                    {
                        tileOnTrigger = tileOnTrigger + Vector3Int.right * seedDirection;
                    }

                    if (posOnTriggerBySeed.y - Mathf.Floor(posOnTriggerBySeed.y) > 0.5f)
                    {
                        tileOnTrigger = tileOnTrigger + Vector3Int.up;
                    }
                    else
                    {
                        tileOnTrigger = tileOnTrigger + Vector3Int.down;
                    }
                }
            }

            if (!gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger + Vector3Int.up) &&
                seedPosY > tileOnTrigger.y + 0.75f)
            {
                seedPosY = tileOnTrigger.y + 0.75f;
            }
            else if (!gameObject.GetComponent<Tilemap>().HasTile(tileOnTrigger + Vector3Int.down) &&
                seedPosY < tileOnTrigger.y + 0.25f)
            {
                seedPosY = tileOnTrigger.y + 0.25f;
            }

            Instantiate(tree, new Vector3(tileOnTrigger.x + 0.5f - 2.5f * seedDirection, seedPosY, 0), Quaternion.identity);
        }
    }
}

