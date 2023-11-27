using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Adjustable variables")]
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
            _audioSource.Play();
        }
    }
}
