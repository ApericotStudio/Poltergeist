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

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (_activated)
        {
            return;
        }
        _activated = true;

        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        if (_initialSound)
        {
            _audioSource.clip = _initialSound;
            _audioSource.Play(0);
            yield return new WaitForSeconds(_initialSound.length);
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
