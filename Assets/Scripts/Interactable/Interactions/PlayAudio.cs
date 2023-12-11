using UnityEngine;

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
    [TextArea]
    public string DevNote = "Other adjustable variables can be found in the audio source component.";

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Toggle()
    {
        if (_onlyActivatable)
        {
            if (_audioClips != null)
            {
                _audioSource.clip = _audioClips.GetRandom();
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
}
