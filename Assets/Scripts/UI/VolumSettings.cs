using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume")){
            LoadVolume();
    }else{
        SetMusicVolume();
        SetSFXVolume();
    }
    }

    public void SetMusicVolume(){
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        
    }

    public void SetSFXVolume(){
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadVolume(){
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
}
