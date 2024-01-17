using UnityEngine;

public class AudioIgnorePause : MonoBehaviour
{
    private void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
    }
}
