using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManeger : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;

    private AudioSource audioSource;
    private GameObject[] musics;


    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
  
   public void SetMusicVolume(float volume)
    {
        musicsource.volume=volume; 
    }

    public void SetButtonVolume(float volume)
    {
        btnsource.volume=volume; 
    }

    public void OnSFX()
    {
        btnsource.Play();
    }
}
