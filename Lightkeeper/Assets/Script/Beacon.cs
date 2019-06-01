using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    InStageManager ISM;

    void Start()
    {
        ISM = GameObject.FindGameObjectWithTag("Managers").GetComponent<InStageManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed") && ISM.beaconCount < 5)
        {
            ISM.beaconHit();
            if(ISM.beaconCount == 5)
            {
                changeSprite();
            }
        }
    }

    void changeSprite()
    {
        SpriteRenderer srNew = gameObject.GetComponent<SpriteRenderer>();
        Sprite BeaconOn = Resources.Load<Sprite>("Sprite/beacon_on");
        srNew.sprite = BeaconOn;
    }

}