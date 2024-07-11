using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManeger : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;

    // public int Length { get; }

/*public void Awake()
{
    var soundManegers=FindObjectOfType<SoundManeger>();

    if(soundManegers.Length ==1 )
    {
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}*/
    private AudioSource audioSource;
    private GameObject[] musics;

    private void Awake()
    {
        musics = GameObject.FindGameObjectsWithTag("Music");

        if(musics.Length >= 2)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

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
