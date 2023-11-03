using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> screamAudioClips;

    public void Scream()
    {
        audioSource.PlayOneShot(screamAudioClips[Random.Range(0, screamAudioClips.Count)]);        
    }
}
