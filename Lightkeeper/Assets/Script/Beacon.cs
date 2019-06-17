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
            
            changeSprite(ISM.beaconCount);
        }
    }

    void changeSprite(int cnt)
    {
        SpriteRenderer srNew = gameObject.GetComponent<SpriteRenderer>();
        Sprite BeaconOn = Resources.Load<Sprite>("Sprite/beacon_on_"+cnt.ToString());
        srNew.sprite = BeaconOn;
    }

}