using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol_point : MonoBehaviour
{
    public Vector3 endPosition;

    void Start()
    {
        endPosition = transform.position;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
