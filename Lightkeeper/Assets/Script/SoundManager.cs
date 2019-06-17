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

    public void soundCarrot()
    {
        myAudio.PlayOneShot(carrot);
    }

    public void soundJump()
    {
        myAudio.PlayOneShot(jump);
    }

    public void soundlighten()
    {
        myAudio.PlayOneShot(lighten);
    }

    public void soundMonsterDeath()
    {
        myAudio.PlayOneShot(monster_death);
    }

    public void soundShoot()
    {
        myAudio.PlayOneShot(shoot);
    }

    public void soundSpring()
    {
        myAudio.PlayOneShot(spring);
    }

    public void soundTree()
    {
        myAudio.PlayOneShot(tree);
    }
}
