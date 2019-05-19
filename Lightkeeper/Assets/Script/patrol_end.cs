using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol_end : MonoBehaviour
{
    public Vector3 endPosition;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        endPosition = transform.position;
    }
}
