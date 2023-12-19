using System.Collections;
using UnityEngine;

public class Sink : MonoBehaviour
{
    [Header("Sink Settings")]
    [Tooltip("The sound that plays when the sink is turned on"), SerializeField]
    private AudioClip _turnOnSound;
    [Tooltip("The sound that plays when the sink is turned off"), SerializeField]
    private AudioClip _turnOffSound;

    private AudioSource _audioSource;


    private ParticleSystem _water;
    private ParticleSystem.MainModule _waterMain;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _water = GetComponentInChildren<ParticleSystem>();
        _waterMain = _water.main;
    }

    public void ToggleWater()
    {
        if (_water.isPlaying)
        {
            AudioSource.PlayClipAtPoint(_turnOffSound, transform.position);
            StartCoroutine(ReduceParticles());
        }
        else
        {
            _waterMain.maxParticles = 1000;
            _water.Play();
            _audioSource.Play();
        }
    }

    private IEnumerator ReduceParticles()
    {
        float duration = _turnOffSound.length/2;
        int initialParticles = _waterMain.maxParticles;
        float reductionRate = initialParticles / duration;

        while (_waterMain.maxParticles > 0)
        {
            _waterMain.maxParticles = Mathf.Max(0, _waterMain.maxParticles - (int)(reductionRate * Time.deltaTime));
            yield return null;
        }
        _audioSource.Stop();
        _water.Stop();
        
    }
}
