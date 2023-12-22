using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Adjustable variables")]
    [Tooltip("Keep empty to use sound clip on audio source component")]
    [SerializeField] private AudioClipList _audioClips = null;
    [Tooltip("Restart sound instead of stopping it")]
    [SerializeField] private bool _onlyActivatable = false;
    [Tooltip("Pause sound instead of stopping it")]
    [SerializeField] private bool _keepProgress = false;
    [Tooltip("Check if item can be turned off or not")]
    [SerializeField] private bool _TurnOff = true;
    [SerializeField] private bool _randomSound = true;

    private int _numberClips;
    private int counter = 0;
    [TextArea]
    public string DevNote = "Other adjustable variables can be found in the audio source component.";

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioClips != null)
        {
            _numberClips = _audioClips.GetAmount();
        }
    }

    public void Toggle()
    {
        if (_onlyActivatable)
        {
            if (_audioClips != null)
            {
                if (_randomSound)
                {
                    _audioSource.clip = _audioClips.GetRandom();
                }
                else
                {
                    OnOff();
                }
            }
            _audioSource.Play();
            return;
        }

        if (_audioSource.isPlaying)
        {
            if (_keepProgress)
            {
                _audioSource.Pause();
            }

            if (!_TurnOff)
            {
                return;
            }

            else
            {
                _audioSource.Stop();
            }
        }
        else
        {
            if (_audioClips != null)
            {
                _audioSource.clip = _audioClips.GetRandom();
            }
            _audioSource.Play();
        }
    }

    public void OnOff()
    {
        if(counter == _numberClips)
        {
            counter = 0;
        }
        _audioSource.clip = _audioClips.GetIndex(counter);
        counter++;
    }
}
