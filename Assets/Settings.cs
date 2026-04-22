using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider SFXSlider;

    private void Start()
    {
        float MasterVal;
        mixer.GetFloat("Master", out MasterVal);
        masterSlider.value = MasterVal;

        float MusicVal;
        mixer.GetFloat("Music", out MusicVal);
        musicSlider.value = MusicVal;

        float SFXrVal;
        mixer.GetFloat("SFX", out SFXrVal);
        SFXSlider.value = SFXrVal;

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

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu_scene");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void pause()
    {
        Time.timeScale = 0f;
    }
    public void unPause()
    {
        Time.timeScale = 1f;
    }
}
