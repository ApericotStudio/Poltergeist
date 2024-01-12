using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCertificateController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private string _creditsSceneName;

    private void Start()
    {
        _audioSource.Play();
        StartCoroutine(WaitForVoiceOver());
    }

    private IEnumerator WaitForVoiceOver()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        GoToCredits();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }

    public void Skip()
    {
        StopAllCoroutines();
        _audioSource.Stop();
        GoToCredits();
    }
}
