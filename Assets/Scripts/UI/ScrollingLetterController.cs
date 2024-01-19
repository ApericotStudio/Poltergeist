using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollingLetterController : MonoBehaviour
{
    [SerializeField]
    private string _levelSelectScene;

    [SerializeField]
    private AudioSource _voiceOverAudio;

    // Start is called before the first frame update
    void Start()
    {
        _voiceOverAudio.Play();
        StartCoroutine(WaitForVoiceOver());
    }

    public void Skip()
    {
        StopAllCoroutines();
        _voiceOverAudio.Stop();
        GoToLevelSelect();
    }

    private IEnumerator WaitForVoiceOver()
    {
        float clipLength = _voiceOverAudio.clip.length;
        yield return new WaitForSeconds(clipLength);
        GoToLevelSelect();
    }

    private void GoToLevelSelect()
    {
        SceneManager.LoadScene(_levelSelectScene);
    }
}
