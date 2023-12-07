using System.Collections;
using UnityEngine;

public class PlayerReactionHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _npcCollection;

    [Header("Adjustable Variables")]
    [Tooltip("Laugh when npc gets scared out of the house")]
    [SerializeField] private AudioClip _teriffiedLaughClip;
    [Tooltip("Laugh when npc gets scared into a new room")]
    [SerializeField] private AudioClip _bigLaughClip;
    [Tooltip("Laugh when npc investigates")]
    [SerializeField] private AudioClip _smallLaughClip;
    [Range(0, 100), Tooltip("Chance that the ghost laughs when an npc investigates")]
    [SerializeField] private int _smallLaughChance = 33;
    [Range(0f, 3f), Tooltip("Delay after scare before ghost laughs")]
    [SerializeField] private float _laughDelay = 0;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SetupListeners();
    }

    private void SetupListeners()
    {
        foreach(AiController npc in _npcCollection.GetComponentsInChildren<AiController>())
        {
            npc.OnStateChange += OnNpcStateChanged;
        }
    }

    private void OnNpcStateChanged(IState state)
    {
        switch (state)
        {
            case (PanickedState):
                {
                    StartCoroutine(PlaySoundAfterDelay(_teriffiedLaughClip, _laughDelay));
                    break;
                }
            case (ScaredState):
                {
                    StartCoroutine(PlaySoundAfterDelay(_bigLaughClip, _laughDelay));
                    break;
                }
            case (InvestigateState):
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
}
