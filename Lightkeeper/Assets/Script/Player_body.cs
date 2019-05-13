using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_body : MonoBehaviour
{
    public bool bodyHit;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CheckPoint")
        {
            char sp = '_';
            string[] splited = collision.gameObject.name.Split(sp);
            Debug.Log("Splited :" + " 0: " + splited[0] + " 1: " + splited[1] + " 2: "+splited[2]);
            PlayerPrefs.SetInt("CheckPoint_" + GameManager.sceneIndex, int.Parse(splited[2]));
            InStageManager.StartingPos = collision.gameObject.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
