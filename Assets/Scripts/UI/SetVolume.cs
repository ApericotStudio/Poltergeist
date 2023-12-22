using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class SetVolume : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        Debug.Log(PlayerPrefsVariable.Volume.ToString());
        float volume = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _audioMixer.SetFloat("GameVol", volume);
    }

    void Start()
    {

    }

    public void ChangeVolume(float sliderValue)
    {
        _audioMixer.SetFloat("GameVol", Mathf.Log10(sliderValue) * 20);
        //PlayerPrefs.SetFloat(PlayerPrefsVariable.Volume.ToString(), sliderValue);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
