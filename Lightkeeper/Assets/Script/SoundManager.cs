using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource myAudio;

    public AudioClip carrot;
    public AudioClip jump;
    //public AudioClip land;
    public AudioClip lighten;
    public AudioClip monster_death;
    public AudioClip shoot;
    public AudioClip spring;
    public AudioClip tree;
    public AudioClip dead;
    public AudioClip save;

    public static SoundManager instance;

    private void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
    }
    
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        
    }

    public void soundCarrot() // Player.cs - OnTriggerEnter2D
    {
        myAudio.PlayOneShot(carrot);
    }

    public void soundJump() // Player.cs - Update
    {
        myAudio.PlayOneShot(jump);
    }

    public void soundlighten() // Beacon.cs
    {
        myAudio.PlayOneShot(lighten);
    }

    public void soundMonsterDeath() // Bat_body.cs, Flower_body.cs
    {
        myAudio.PlayOneShot(monster_death);
    }

    public void soundShoot() // Player.cs - Shot
    {
        myAudio.PlayOneShot(shoot);
    }

    public void soundSpring() // Player.cs - Move, Update
    {
        myAudio.PlayOneShot(spring);
    }

    public void soundTree() // Dirt.cs
    {
        myAudio.PlayOneShot(tree);
    }

    public void soundDead() // Player.cs
    {
        myAudio.PlayOneShot(dead);
    }

    public void soundSave()
    {
        myAudio.PlayOneShot(save);
    }
}
