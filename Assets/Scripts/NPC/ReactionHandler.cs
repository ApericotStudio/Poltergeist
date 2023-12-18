using System;
using UnityEngine;
using UnityEngine.UI;

public class ReactionHandler : MonoBehaviour
{
    [Header("Reaction Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC gets scared into a new room."), SerializeField]
    private AudioClipList _bigScreamAudioClips;
    [Tooltip("The audio clips that will be played when the NPC screams."), SerializeField]
    private AudioClipList _terrifiedAudioClips;
    [Tooltip("The audio clip that will be played when the NPC investigates."), SerializeField]
    private AudioClipList _investigateAudioClips;
    [Tooltip("The audio clip that will be played when the NPC stops investigating."), SerializeField]
    private AudioClipList _investigateEndAudioClips;
    
    [Header("Reaction Indicator Settings")]
    [Tooltip("The image that will be used to display the reaction sprite."), SerializeField]
    private Image _reactionImage;
    [Tooltip("The particle system that will be played when the NPC is panicked or hit the sweaty treshold."), SerializeField]
    private ParticleSystem _particleSystem;

    [Header("Reaction Tresholds")]
    [Tooltip("The fear value at which the NPC will start to sweat."), Range(0f, 100f), SerializeField]
    private float _sweatTreshold = 66f;
    [Tooltip("The fear value at which the NPC will start to get grumpy."), Range(0f, 100f), SerializeField]
    private float _grumpyTreshold = 33f;

    [Header("Chance to play voiceline on state enter")]
    [SerializeField, Range(0, 100)]
    private int _investigateVoicelineChance = 40;
    private int _investigateVoiceLineUnsuccesfullAttempts = 0;
    [SerializeField, Range(0, 100)]
    private int _investigateEndVoicelineChance = 40;
    private int _investigateEndVoiceLineUnsuccesfullAttempts = 0;
    private float _voicelineChanceIncreaseMultiplier = 5; //increases how much the probability increases with every failed attempt

    [Header("Faces")]
    [Tooltip("The NPC's face mesh, used for showing their reaction."), SerializeField]
    private SkinnedMeshRenderer _faceMesh;
    [Tooltip("This face gets shown when the NPC's is idling"), SerializeField]
    private Material _restingFace;
    [Tooltip("This face gets shown when the NPC is scared"), SerializeField]
    private Material _scaredFace;
    [Tooltip("This face gets shown when the NPC is investigating"), SerializeField]
    private Material _investigateFace;
    [Tooltip("This face gets shown when the NPC is panicked"), SerializeField]
    private Material _panickedFace;

    private IState _previousState;
    private Animator _animator;
    private NpcController _npcController;

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        _animator = _reactionImage.GetComponent<Animator>();
        _npcController.OnStateChange.AddListener(OnStateChange);

        if(_npcController.TryGetComponent(out VisitorController visitorController))
        {
            visitorController.OnFearValueChange.AddListener(OnFearValueChange);
        }

        _previousState = _npcController.CurrentState;
        
    }

    private void OnFearValueChange(float fear)
    {
        switch(fear)
        {
            case float f when f >= _sweatTreshold:
                _animator.SetBool("Grumpy", false);
                _particleSystem.Play();
                break;
            case float f when f >= _grumpyTreshold:
                _animator.SetBool("Grumpy", true);
                _particleSystem.Stop();
                break;
            default:
                _animator.SetBool("Grumpy", false);
                _particleSystem.Stop();
                break;
        }
    }

    private void OnStateChange(IState state)
    {
        PlayReaction();
        ChangeFace();
        _previousState = state;
    }
    
    private void PlayReaction()
    {
        AudioClip clip = null;

        switch (_npcController.CurrentState)
        {
            case InvestigateState when _previousState is not ScaredState && _previousState is not PhobiaState:
                TryPlayVoiceline(_investigateAudioClips.GetRandom(), _investigateVoicelineChance, ref _investigateVoiceLineUnsuccesfullAttempts);
                ToggleAnimation("Investigating");
                break;
            case RoamState when _previousState is InvestigateState:
                TryPlayVoiceline(_investigateEndAudioClips.GetRandom(), _investigateEndVoicelineChance, ref _investigateEndVoiceLineUnsuccesfullAttempts);
                ToggleAnimation("Investigating");
                break;
            case RoamState when _previousState is ScaredState:
                ToggleAnimation("Scared");
                break;
            case RoamState when _previousState is PhobiaState:
                ToggleAnimation("Phobia");
                break;
            case PanickedState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _terrifiedAudioClips.GetRandom();
                break;
            case ScaredState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _bigScreamAudioClips.GetRandom();
                ToggleAnimation("Scared");
                break;
            case PhobiaState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _terrifiedAudioClips.GetRandom();
                ToggleAnimation("Phobia");
                break;
            case CheckUpState when _previousState is InvestigateState:
                ToggleAnimation("Investigating");
                break;
        }

        if (clip != null)
        {
            _npcController.AudioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Toggles the animation with the given name while disabling all other animations.
    /// </summary>
    private void ToggleAnimation(string animationName)
    {
        string[] listOfAnimations = new[] {"Investigating", "Scared", "Phobia"};

        foreach (string anim in listOfAnimations)
            if (anim != animationName)
                _animator.SetBool(anim, false);

        _animator.SetBool(animationName, !_animator.GetBool(animationName));
    }

    private void ChangeFace()
    {
        if (_faceMesh != null)
        {
            switch (_npcController.CurrentState)
            {
                case InvestigateState:
                    SetFace(_investigateFace);
                    break;
                case RoamState:
                    SetFace(_restingFace);
                    break;
                case PanickedState:
                    SetFace(_panickedFace);
                    break;
                case ScaredState:
                    SetFace(_scaredFace);
                    break;
            }
        }
    }

    private void SetFace(Material newFace)
    {
        _faceMesh.material = newFace;
    }

    private void TryPlayVoiceline(AudioClip clip, int baseChance, ref int unsuccesfullAttempts)
    {
        bool playVoiceline = UnityEngine.Random.Range(1, 100) < baseChance + Mathf.Pow(unsuccesfullAttempts, _voicelineChanceIncreaseMultiplier * baseChance / 100);
        if (playVoiceline)
        {
            _npcController.AudioSource.PlayOneShot(clip);
            unsuccesfullAttempts = 0;
        }
        else
        {
            unsuccesfullAttempts++;
        }
    }
}
