using System.Collections;
using UnityEngine;

public class HairDryer : MonoBehaviour
{
    [Header("Hair dryer settings")]
    [Tooltip("Sound that plays when hair dryer is turned on"), SerializeField]
    private AudioClip _turnOnSound;
    [Tooltip("Sound that plays when hair dryer is turned off"), SerializeField]
    private AudioClip _turnOffSound;
    [Tooltip("Sound that plays when hair dryer is running"), SerializeField]
    private AudioClip _hairDryerRunningSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Toggle()
    {
        if (_audioSource.isPlaying)
        {
            TurnOff();
        }
        else
        {
            StartCoroutine(TurnOn());
        }
    }

    private IEnumerator TurnOn()
    {
        AudioSource.PlayClipAtPoint(_turnOnSound, transform.position);
        yield return new WaitForSeconds(_turnOnSound.length);
        _audioSource.Play();
    }

    private void TurnOff()
    {
        AudioSource.PlayClipAtPoint(_turnOffSound, transform.position);
        _audioSource.Stop();
    }

}
