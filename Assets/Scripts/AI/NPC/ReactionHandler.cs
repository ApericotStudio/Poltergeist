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

    [Header("Reaction Image Settings")]
    [Tooltip("The image that will be used to display the reaction sprite."), SerializeField]
    private Image _reactionImage;
    [Tooltip("The sprite that will be displayed when the fear value is low."), SerializeField]
    private Sprite _lowFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is medium."), SerializeField]
    private Sprite _mediumFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is high."), SerializeField]
    private Sprite _highFearSprite;
    [Tooltip("The sprite that will be displayed when the NPC is investigating."), SerializeField]
    private Sprite _investigateSprite;
    [Tooltip("The sprite that will be displayed when the NPC is scared."), SerializeField]
    private Sprite _scaredSprite;

    [Header("Faces")]
    [SerializeField] private Material _restingFace;
    [SerializeField] private Material _scaredFace;
    [SerializeField] private Material _investigateFace;
    [SerializeField] private Material _panickedFace;
    [SerializeField] private SkinnedMeshRenderer _faceMesh;

    private NpcController _npcController;
    private IState _previousState;

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        _npcController.OnStateChange += OnStateChange;
        _npcController.OnFearValueChange.AddListener(OnFearValueChange);
        _previousState = _npcController.CurrentState;
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

        switch (_npcController.CurrentState)
        {
            case InvestigateState when _previousState is RoamState:
                clip = _investigateAudioClips.GetRandom();
                break;
            case RoamState when _previousState is InvestigateState:
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
            _npcController.NpcAudioSource.PlayOneShot(clip);
        }
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

    private void SetReactionSpriteBasedOnState()
    {
        if(_npcController.CurrentState is InvestigateState)
        {
            _reactionImage.sprite = _investigateSprite;
        }
        else if(_npcController.CurrentState is ScaredState)
        {
            _reactionImage.sprite = _scaredSprite;
        }
        else
        {
            SetReactionSpriteBasedOnFear(_npcController.FearValue);
        }
    }

    private void SetReactionSpriteBasedOnFear(float fear)
    {
        if(_npcController.CurrentState is InvestigateState || _npcController.CurrentState is ScaredState)
        {
            return;
        }

        if(fear <= 25f)
        {
            _reactionImage.sprite = _lowFearSprite;
        }
        else if(fear >= 75f)
        {
            _reactionImage.sprite = _highFearSprite;
        }
        else
        {
            _reactionImage.sprite = _mediumFearSprite;
        }
    }
}
