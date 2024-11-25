using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//gh
public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private SoundInfo _soundinfo;
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider Master;
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider SFX;

    private void OnEnable()
    {
        Master.value = _soundinfo.Master_value;
        BGM.value = _soundinfo.BGMvalue;
        SFX.value = _soundinfo.SFXvalue;
    }

    public void SetMasterVolume()
    {
        float sound = Master.value;

        if (sound == 0) Mixer.SetFloat("Master", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("Master", sound);
        }

        _soundinfo.Master_value = (int)(Master.value);
    }

    public void SetBGMVolume()
    {
        float sound = BGM.value;

        if (sound == 0) Mixer.SetFloat("BGM", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("BGM", sound);
        }

        _soundinfo.BGMvalue = (int)(BGM.value);
    }

    public void SetEffectVolume()
    {

        float sound = SFX.value;

        if (sound == 0) Mixer.SetFloat("SFX", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("SFX", sound);
        }

        _soundinfo.SFXvalue = (int)(SFX.value);
    }

}
