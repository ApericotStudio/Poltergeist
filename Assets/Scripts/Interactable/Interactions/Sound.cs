using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    private AudioClip _delayedSound;
    [SerializeField]
    private AudioClip _initialSound;

    private AudioSource _audioSource;

    private bool _activated = false;

    private IEnumerator _playSoundFunction;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if (_activated)
        {
            if (_playSoundFunction != null) StopCoroutine(_playSoundFunction);
            _audioSource.Stop();
            _activated = false;
            return;
        }
        _activated = true;
        _playSoundFunction = PlaySound();
        StartCoroutine(_playSoundFunction);
    }

    IEnumerator PlaySound()
    {
        if (_initialSound)
        {
            _audioSource.clip = _initialSound;
            _audioSource.Play(0);
            yield return new WaitForSeconds(5f);
            _audioSource.Stop();
        }

        if (_delayedSound)
        {
            _audioSource.clip = _delayedSound;
            _audioSource.Play();
            yield return new WaitForSeconds(_delayedSound.length);
            _audioSource.Stop();
        }

        _activated = false;
    }
}
