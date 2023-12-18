using UnityEngine;

public class Fridge : MonoBehaviour
{
    [Header("Fridge Settings")]
    [Tooltip("Sound played when the fridge is opened."), SerializeField]
    private AudioClip _openSound;
    [Tooltip("Sound played when the fridge is closed."), SerializeField]
    private AudioClip _closeSound;
    
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
            AudioSource.PlayClipAtPoint(_openSound, transform.position);
    }

    private void OnClose()
    {
        _highlight.Highlightable(true);

        if(_closeSound != null)
            AudioSource.PlayClipAtPoint(_closeSound, transform.position);
    }
}