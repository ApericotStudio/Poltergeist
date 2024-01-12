using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCertificateController : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private readonly string _creditsSceneName;

    private void Start()
    {
        StartCoroutine(WaitForVoiceOver());
    }

    private IEnumerator WaitForVoiceOver()
    {
        yield return new WaitForSeconds(_clip.length);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }
}
