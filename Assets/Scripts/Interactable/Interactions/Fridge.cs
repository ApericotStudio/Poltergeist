using UnityEngine;
using UnityEngine.Audio;

public class Fridge : MonoBehaviour
{
    [Header("Fridge Settings")]
    [Tooltip("Sound played when the fridge is opened."), SerializeField]
    private AudioClip _openSound;
    [Tooltip("Sound played when the fridge is closed."), SerializeField]
    private AudioClip _closeSound;
    [Tooltip("The AudioMixerGroup that the sounds will play on"), SerializeField]
    private AudioMixerGroup _mixerGroup;
    
    private Animator _animator;
    private Highlight _highlight;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _highlight = GetComponent<Highlight>();
    }

    public void Activate()
    {
        _animator.SetTrigger("Open");
    }

    private void OnOpen()
    {
        _highlight.Highlightable(false);

        if (_openSound != null)
        {
            _mixerGroup.audioMixer.GetFloat("GameVol", out float volume);
            volume = Mathf.Pow(10, volume / 20);
            AudioSource.PlayClipAtPoint(_openSound, transform.position, volume);
        }
    }

    private void OnClose()
    {
        _highlight.Highlightable(true);
        _highlight.Highlighted(true);

        if(_closeSound != null)
        {
            _mixerGroup.audioMixer.GetFloat("GameVol", out float volume);
            volume = Mathf.Pow(10, volume / 20);
            AudioSource.PlayClipAtPoint(_closeSound, transform.position, volume);
        }
            
    }
}