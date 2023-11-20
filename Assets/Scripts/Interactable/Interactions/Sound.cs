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

        StartCoroutine(PlaySound());

        _activated = false;
    }

    IEnumerator PlaySound()
    {
        if (_audioSource.clip)
        {
            _audioSource.Play(0);
            yield return new WaitForSeconds(_audioSource.clip.length);
            _audioSource.Stop();
        }

        if (_delayedSound)
        {
            _audioSource.PlayOneShot(_delayedSound);
        }

    }
}
