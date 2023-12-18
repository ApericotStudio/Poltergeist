using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    //other scripts
    private AudioSource _audioSource;
    private ObjectWithDoor _door;

    [SerializeField] private AudioClip _startSound;
    [SerializeField] private AudioClip _busySound;
    [SerializeField] private AudioClip _stopSound;
    private bool _turnedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.clip = _busySound;
        _door = this.GetComponent<ObjectWithDoor>();
    }

    public void Toggle()
    {
        if (_turnedOn) { turnOff(); }
        else { turnOn(); }
    }

    private void turnOn()
    {
        _turnedOn = true;
        _door.Close();
        _audioSource.PlayOneShot(_startSound);
        StartCoroutine(OvenBusy());
    }

    private void turnOff()
    {
        _turnedOn = false;
        StartCoroutine(OvenStopping());
    }

    private IEnumerator OvenBusy()
    {
        yield return new WaitForSeconds(2);
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private IEnumerator OvenStopping()
    {
        _audioSource.loop = false;
        _audioSource.PlayOneShot(_stopSound);
        yield return new WaitWhile(() => _audioSource.isPlaying);
        _door.Open();
    }
}
