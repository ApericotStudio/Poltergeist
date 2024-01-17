using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishAudioOnLoad : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SceneManager.sceneUnloaded += FinishPlaying;
        DontDestroyOnLoad(gameObject);
    }

    private void FinishPlaying(Scene scene)
    {
        SceneManager.sceneUnloaded -= FinishPlaying;
        if (_audioSource.isPlaying)
        {
            Destroy(gameObject, _audioSource.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
