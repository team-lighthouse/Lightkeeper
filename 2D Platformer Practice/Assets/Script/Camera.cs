using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    
    void LateUpdate()
    {
        if (player.transform.position.y > 0)
        {
            transform.position = new Vector3(0, player.transform.position.y, -10);
        }
    }
}
