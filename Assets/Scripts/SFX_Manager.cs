using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public static SFX_Manager instance;

    [Header("SFX")]
    [SerializeField]
    private AudioSource _sfxPlayer;
    [SerializeField]
    private AudioClip[] _sfxClips;

    private void Awake()
    {
        instance = this;
    }

    public void Set_SFX(int i)
    {
        _sfxPlayer.Pause();
        _sfxPlayer.clip = null;
        _sfxPlayer.clip = _sfxClips[i];
        _sfxPlayer.Play();
    }

    public void Stop_SFX()
    {
        _sfxPlayer.Stop();
    }

    public void Play_oneshot(AudioClip clip)
    {
        _sfxPlayer.PlayOneShot(clip);
    }
}
