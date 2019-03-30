using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("TreeCounter").GetComponent<TreeCounter>().trees.Enqueue(gameObject);
    }
}
