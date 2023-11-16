using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Adjustable variables")]
    [Tooltip("Pause sound instead of stopping it")]
    [SerializeField] private bool _keepProgress = false;
    [TextArea]
    public string DevNote = "Other adjustable variables can be found in the audio source component.";

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Toggle()
    {
        if (_audioSource.isPlaying)
        {
            if (_keepProgress)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.Stop();
            }
        }
        else
        {
            _audioSource.Play();
        }
    }
}
