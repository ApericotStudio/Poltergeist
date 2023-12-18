using System.Collections;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    [Header("Toilet Settings")]
    [Tooltip("The sound that plays when the toilet gets flushed"), SerializeField]
    private AudioClip _flushSound;

    private AudioSource _audioSource;
    private ParticleSystem _water;
    private ParticleSystem.MainModule _waterMain;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _water = GetComponentInChildren<ParticleSystem>();
        _waterMain = _water.main;
    }

    public void Flush()
    {
        StartCoroutine(FlushRoutine());
    }

    private IEnumerator FlushRoutine()
    {
        if(!_water.isPlaying)
        {
            _waterMain.maxParticles = 1000;
            _audioSource.PlayOneShot(_flushSound);
            _water.Play();
            yield return new WaitForSeconds(_flushSound.length/1.5f);
            StartCoroutine(ReduceParticles());
        }
    }

    private IEnumerator ReduceParticles()
    {
        float duration = _flushSound.length / 4;
        int initialParticles = _waterMain.maxParticles;
        float reductionRate = initialParticles / duration;

        while (_waterMain.maxParticles > 0)
        {
            _waterMain.maxParticles = Mathf.Max(0, _waterMain.maxParticles - (int)(reductionRate * Time.deltaTime));
            yield return null;
        }
        _water.Stop();
    }
}