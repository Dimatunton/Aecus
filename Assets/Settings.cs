using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider SFXSlider;

    private void Start()
    {
        onMasterSliderChanged();
        onmusicSliderChanged();
        onSFXSliderChanged();
    }

    public void onMasterSliderChanged()
    {
        mixer.SetFloat("Master", masterSlider.value);
    }

    public void onmusicSliderChanged()
    {
        mixer.SetFloat("Music", musicSlider.value);
    }
    public void onSFXSliderChanged()
    {
        mixer.SetFloat("SFX", SFXSlider.value);
    }
}
