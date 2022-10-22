using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public Color volumeMaster;
    public Color volumeBGM;
    public Color volumeSFX;

    public AudioMixer am;

    public void SetMasterVolume(float sliderValue)
    {
        am.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetBGMVolume(float sliderValue)
    {
        am.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        am.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
}
