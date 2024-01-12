using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndCutsceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private string _creditsSceneName; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForVideoComplete());
    }

    private IEnumerator WaitForVideoComplete()
    {
        yield return new WaitForSeconds((float)_video.length);
        ContinueToCreditsAfterVideo();
    }

    public void ContinueToCreditsAfterVideo()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }
}
