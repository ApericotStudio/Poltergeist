using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    private float _amountInitialTime;
    [SerializeField]
    private float _amountDelayedTime;
    [SerializeField]
    private AudioClip _delayedSound;
    [SerializeField]
    private AudioClip _initialSound;
    [SerializeField]
    private AudioSource _audioSource;

    private bool _activated = false;

    // Start is called before the first frame update
    void Start()
    {
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
        
        if (_initialSound)
        {
            StartCoroutine(PlayInitialSound());
        }

        if (_delayedSound)
        {
            StartCoroutine(PlayDelayedSound());
        }

        _audioSource.Stop();
        _activated = false;
    }

    IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(_amountDelayedTime);

        _audioSource.Stop();

        _audioSource.PlayOneShot(_delayedSound);
    }

    IEnumerator PlayInitialSound()
    {
        _audioSource.Play(0);

        yield return null;
    }
}
