using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCounter : MonoBehaviour
{
    int treeLimit;
    public Queue<GameObject> trees = new Queue<GameObject>();

    void Start()
    {
        treeLimit = 3;
    }
    
    void Update()
    {
        if (trees.Count > treeLimit)
        {
            Destroy(trees.Dequeue());
        }
    }
}
