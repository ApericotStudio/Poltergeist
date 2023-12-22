using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private AudioMixer _audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = PlayerPrefs.GetInt(PlayerPrefsVariable.Fullscreen.ToString(), Screen.fullScreen ? 1 : 0) == 1;
        Screen.brightness = PlayerPrefs.GetInt(PlayerPrefsVariable.Brightness.ToString(), 100);
        float volume = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _audioMixer.SetFloat("GameVol", volume);
    }

    // Update is called once per frame
    void Update()
    {
        _audioMixer.GetFloat("GameVol",out float volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log(isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

    public void SetBrightness(float sliderValue)
    {
        Screen.brightness = sliderValue;
    }

    public void SetContrast(float sliderValue)
    {

    }

    public void ChangeVolume(float sliderValue)
    {
        _audioMixer.SetFloat("GameVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(PlayerPrefsVariable.Volume.ToString(), sliderValue);
        PlayerPrefs.Save();
    }
}
