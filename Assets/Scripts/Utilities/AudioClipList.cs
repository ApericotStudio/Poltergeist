using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioClipList", menuName = "ScriptableObjects/AudioClipList", order = 1)]
public class AudioClipList : ScriptableObject
{
    [SerializeField] private List<AudioClip> _audioClips = new List<AudioClip>();

    public AudioClip GetRandom()
    {
        if (_audioClips.Count == 0)
        {
            throw new ArgumentOutOfRangeException("List " + name + " is empty");
        }
        int randomIndex = UnityEngine.Random.Range(0, _audioClips.Count);
        return _audioClips[randomIndex];
    }
}
