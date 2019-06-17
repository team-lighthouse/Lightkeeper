using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    InStageManager ISM;
    new SpriteRenderer renderer;
    public Sprite beaconOff;
    public Sprite beaconOn1;
    public Sprite beaconOn2;
    public Sprite beaconOn3;
    public Sprite beaconOn4;
    public Sprite beaconOn5;

    void Start()
    {
        ISM = GameObject.FindGameObjectWithTag("Managers").GetComponent<InStageManager>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed") && ISM.beaconCount < 5)
        {
            ISM.beaconHit();
            SoundManager.instance.soundlighten();

            if (ISM.beaconCount == 1)
            {
                renderer.sprite = beaconOn1;
            }
            else if (ISM.beaconCount == 2)
            {
                renderer.sprite = beaconOn2;
            }
            else if (ISM.beaconCount == 3)
            {
                renderer.sprite = beaconOn3;
            }
            else if (ISM.beaconCount == 4)
            {
                renderer.sprite = beaconOn4;
            }
            else if (ISM.beaconCount == 5)
            {
                renderer.sprite = beaconOn5;
            }
        }
    }

}