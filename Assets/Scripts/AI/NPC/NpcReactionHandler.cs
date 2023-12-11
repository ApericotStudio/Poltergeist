using UnityEngine;
using UnityEngine.UI;

public class NpcReactionHandler : MonoBehaviour
{
    [Header("Reaction Audio Settings")]
    [Tooltip("The audio clip that will be played when the NPC gets a big scare."), SerializeField]
    private AudioClipList _bigScreamAudioClips;
    [Tooltip("The audio clip that will be played when the NPC gets the final scare."), SerializeField]
    private AudioClipList _terrifiedAudioClips;
    [Tooltip("The audio clip that will be played when the NPC investigates."), SerializeField]
    private AudioClipList _investigateAudioClips;
    [Tooltip("The audio clip that will be played when the NPC stops investigating."), SerializeField]
    private AudioClipList _investigateEndAudioClips;

    [Header("Reaction Image Settings")]
    [Tooltip("The image that will be used to display the reaction sprite."), SerializeField]
    private Image _reactionImage;
    [Tooltip("The sprite that will be displayed when the NPC is investigating."), SerializeField]
    private Sprite _investigateSprite;
    [Tooltip(""), SerializeField]
    private Sprite _scaredSprite;
    [Tooltip(""), SerializeField]
    private Sprite _lowFearSprite;
    [Tooltip(""), SerializeField]
    private Sprite _mediumFearSprite;
    [Tooltip(""), SerializeField]
    private Sprite _highFearSprite;

    [Header("Faces")]
    [SerializeField] private Material _restingFace;
    [SerializeField] private Material _investigateFace;
    [SerializeField] private Material _scaredFace;
    [SerializeField] private Material _panickedFace;
    [SerializeField] private SkinnedMeshRenderer _faceMesh;

    private AiController _aiController;
    private NpcController _npcController;
    private IState _previousState;

    private void Awake()
    {
        _aiController = GetComponent<AiController>();
        _npcController = GetComponent<NpcController>();
        _aiController.OnStateChange += OnStateChange;
        _previousState = _aiController.CurrentState;
    }

    private void OnStateChange(IState state)
    {
        PlayReactionSound();
        SetReactionSpriteBasedOnState();
        ChangeFace();
        _previousState = state;
    }


    private void OnFearValueChange(float fear)
    {
        SetReactionSpriteBasedOnFear(fear);
    }

    private void PlayReactionSound()
    {
        AudioClip clip = null;

        switch (_aiController.CurrentState)
        {
            case InvestigateState when _previousState is RoamState:
                clip = _investigateAudioClips.GetRandom();
                break;
            case RoamState when _previousState is InvestigateState:
                clip = _investigateEndAudioClips.GetRandom();
                break;
            case InvestigateState when _previousState is CheckUpState:
                clip = _investigateAudioClips.GetRandom();
                break;
            case CheckUpState when _previousState is InvestigateState:
                clip = _investigateEndAudioClips.GetRandom();
                break;
            case PanickedState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _terrifiedAudioClips.GetRandom();
                break;
            case ScaredState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _bigScreamAudioClips.GetRandom();
                break;
        }

        if (clip != null)
        {
            _aiController.AiAudioSource.PlayOneShot(clip);
        }
    }

    private void ChangeFace()
    {
        if (_faceMesh != null)
        {
            switch (_aiController.CurrentState)
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

    private void SetReactionSpriteBasedOnState()
    {
        if (_aiController.CurrentState is InvestigateState)
        {
            _reactionImage.enabled = true;
            _reactionImage.sprite = _investigateSprite;
        }
        else if (_aiController.CurrentState is ScaredState)
        {
            _reactionImage.enabled = true;
            _reactionImage.sprite = _scaredSprite;
        }
        else if (_npcController)
        {
            SetReactionSpriteBasedOnFear(_npcController.FearValue);
        }
        else
        {
            _reactionImage.enabled = false;
        }
    }

    private void SetReactionSpriteBasedOnFear(float fear)
    {
        if(_aiController.CurrentState is InvestigateState || _aiController.CurrentState is ScaredState)
        {
            return;
        }

        if(fear <= 25f)
        {
            _reactionImage.enabled = true;
            _reactionImage.sprite = _lowFearSprite;
        }
        else if(fear >= 75f)
        {
            _reactionImage.enabled = true;
            _reactionImage.sprite = _highFearSprite;
        }
        else
        {
            _reactionImage.enabled = true;
            _reactionImage.sprite = _mediumFearSprite;
        }
    }
}
