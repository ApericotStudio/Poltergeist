using UnityEngine;
using UnityEngine.UI;

public class ReactionController : MonoBehaviour
{
    [Header("Reaction Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC gets scared into a new room.")]
    public AudioClipList SmallScreamAudioClips;
    [Tooltip("The audio clips that will be played when the NPC screams.")]
    public AudioClipList ScreamAudioClips;
    [Tooltip("The audio clip that will be played when the NPC investigates.")]
    public AudioClipList InvestigateAudioClips;
    [Tooltip("The audio clip that will be played when the NPC stops investigating.")]
    public AudioClipList InvestigateEndAudioClips;
    [Tooltip("The volume of the scream audio clips.")]
    [Range(0f, 1f)]
    public float ScreamVolume = 1f;
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

    private NpcController _npcController;
    private IState _previousState;

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        _npcController.OnStateChange.AddListener(OnStateChange);
        _npcController.OnFearValueChange.AddListener(OnFearValueChange);
        _previousState = _npcController.CurrentState;
    }

    private void OnStateChange()
    {
        PlayReactionSound();
        SetReactionSpriteBasedOnState();
        _previousState = _npcController.CurrentState;
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
                clip = InvestigateAudioClips.GetRandom();
                break;
            case RoamState when _previousState is InvestigateState:
                clip = InvestigateEndAudioClips.GetRandom();
                break;
            case PanickedState when _previousState is RoamState || _previousState is InvestigateState:
                clip = ScreamAudioClips.GetRandom();
                break;
            case ScaredState when _previousState is RoamState || _previousState is InvestigateState:
                clip = SmallScreamAudioClips.GetRandom();
                break;
        }

        if (clip != null)
        {
            _npcController.NpcAudioSource.PlayOneShot(clip);
        }
    }

    private void SetReactionSpriteBasedOnState()
    {
        if(_npcController.CurrentState == _npcController.InvestigateState)
        {
            _reactionImage.sprite = _investigateSprite;
        }
        else if(_npcController.CurrentState == _npcController.ScaredState)
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
        if(_npcController.CurrentState is InvestigateState)
        {
            return;
        }
        if(_npcController.CurrentState is ScaredState)
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
