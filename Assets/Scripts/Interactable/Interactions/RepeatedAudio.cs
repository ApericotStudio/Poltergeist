using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatedAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip _soundEffect;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSource.PlayOneShot(_soundEffect);
    }
}
