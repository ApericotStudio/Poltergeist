using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    public static SoundtrackController Instance;

    private AudioSource _audioSource;

    [SerializeField] private bool _noSoundtrack;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (Instance != null && Instance != this)
        {
            if (_noSoundtrack)
            {
                Instance._audioSource.Pause();
            }
            else
            {
                Instance.PlayAudioClip(_audioSource.clip);
            }
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (!_noSoundtrack)
            {
                PlayAudioClip(_audioSource.clip);
            }
        }
    }

    /// <summary>
    /// Play correct sound track
    /// </summary>
    public void PlayAudioClip(AudioClip clip)
    {
        if (!Instance._audioSource.isPlaying)
        {
            Instance._audioSource.Play();
        }
        if (_audioSource.clip == clip)
        {
            return;
        }
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
