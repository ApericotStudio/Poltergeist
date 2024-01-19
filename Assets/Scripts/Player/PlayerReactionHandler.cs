using System.Collections;
using UnityEngine;

public class PlayerReactionHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _visitorCollection;

    [Header("Adjustable Variables")]
    [Tooltip("Laugh when visitor gets scared out of the house")]
    [SerializeField] private AudioClip _teriffiedLaughClip;
    [Tooltip("Laugh when visitor gets scared into a new room")]
    [SerializeField] private AudioClip _bigLaughClip;
    [Tooltip("Laugh when visitor investigates")]
    [SerializeField] private AudioClip _smallLaughClip;
    [Range(0, 100), Tooltip("Chance that the ghost laughs when a visitor investigates")]
    [SerializeField] private int _smallLaughChance = 33;
    [Range(0f, 3f), Tooltip("Delay after scare before ghost laughs")]
    [SerializeField] private float _laughDelay = 0;

    private AudioSource _audioSource;

    [Header("Ghost Faces")]
    [SerializeField] private MeshRenderer _faceMesh;
    [SerializeField] private Material _ghostResting;
    [SerializeField] private Material _ghostConfused;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SetupListeners();
    }

    private void SetupListeners()
    {
        foreach(VisitorController visitor in _visitorCollection.GetComponentsInChildren<VisitorController>())
        {
            visitor.OnStateChange.AddListener(OnVisitorStateChanged);
        }
    }

    private void OnVisitorStateChanged(IState state)
    {
        switch (state)
        {
            case PanickedState:
                {
                    StartCoroutine(PlaySoundAfterDelay(_teriffiedLaughClip, _laughDelay));
                    break;
                }
            case ScaredState:
                {
                    StartCoroutine(PlaySoundAfterDelay(_bigLaughClip, _laughDelay));
                    break;
                }
            case InvestigateState:
                {
                    if (Random.Range(0, 101) > _smallLaughChance)
                    {
                        break;
                    }
                    StartCoroutine(PlaySoundAfterDelay(_smallLaughClip, _laughDelay));
                    break;
                }
        }
    }

    private IEnumerator PlaySoundAfterDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void ToggleFace()
    {
        if (_faceMesh != null)
        {
            if (_faceMesh.sharedMaterial == _ghostResting)
            {
                _faceMesh.sharedMaterial = _ghostConfused;
            } else
            {
                _faceMesh.sharedMaterial = _ghostResting;
            }
        }
    }
}
