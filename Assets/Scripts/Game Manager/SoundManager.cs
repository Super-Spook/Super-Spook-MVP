using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip Dash_Audio, Jump_Audio, MushroomJump_Audio, MovingPlatform_Audio;
    private AudioSource audioSource;
    public bool PlatformSoundPlaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlatformSoundPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string clip) 
    {
        switch (clip)
        {
            case "Dash":
                audioSource.PlayOneShot(Dash_Audio);
                break;
            case "Jump":
                audioSource.PlayOneShot(Jump_Audio);
                break;
            case "MushroomJump":
                audioSource.PlayOneShot(MushroomJump_Audio);
                break;
        }
    }
    public void PlayLoopingSound(string clip)
    {
        switch (clip)
        {
            case "MovingPlatform":
                if (!PlatformSoundPlaying) 
                {
                    audioSource.clip = MovingPlatform_Audio;
                    audioSource.Play();
                    PlatformSoundPlaying = true;
                    audioSource.loop = true;
                }              
                break;
        }
    }
}
