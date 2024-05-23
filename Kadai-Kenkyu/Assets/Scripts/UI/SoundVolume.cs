using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderBGM;
    public Slider sliderSE;

    private void Start()
    {
        audioMixer.GetFloat("BGM_volume", out float bgmVolume);
        sliderBGM.value = bgmVolume;
        audioMixer.GetFloat("SE_volume", out float seVolume);
        sliderSE.value = seVolume;
    }

    public void SetBGM(float _volume)
    {
        audioMixer.SetFloat("BGM_volume", _volume);
    }
    
    public void SetSE(float _volume)
    {
        audioMixer.SetFloat("SE_volume", _volume);
    }
}
