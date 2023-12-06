using UnityEngine;

public class PlayerReactionHandler : MonoBehaviour
{
    [SerializeField] private GameObject _npcCollection;
    [SerializeField] private AudioClip _teriffiedLaughClip;
    [SerializeField] private AudioClip _bigLaughClip;
    [SerializeField] private AudioClip _smallLaughClip;

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
                    _audioSource.clip = _teriffiedLaughClip;
                    _audioSource.Play();
                    break;
                }
            case (ScaredState):
                {
                    _audioSource.clip = _bigLaughClip;
                    _audioSource.Play();
                    break;
                }
            case (InvestigateState):
                {
                    _audioSource.clip = _smallLaughClip;
                    _audioSource.Play();
                    break;
                }
        }
    }
}
