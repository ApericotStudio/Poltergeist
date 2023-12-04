using UnityEngine;
using UnityEngine.UI;

public class ReactionController : MonoBehaviour
{
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
        if(_previousState is RoamState && _npcController.CurrentState is InvestigateState)
        {
            _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateAudioClips.GetRandom());
        }
        else if(_previousState is InvestigateState && _npcController.CurrentState is RoamState)
        {
            _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateEndAudioClips.GetRandom());
        }
        else if(_previousState is not PanickedState && _npcController.CurrentState is PanickedState)
        {
            _npcController.NpcAudioSource.PlayOneShot(_npcController.ScreamAudioClips.GetRandom());
        }
        else if(_previousState is not ScaredState && _npcController.CurrentState is ScaredState)
        {
            _npcController.NpcAudioSource.PlayOneShot(_npcController.SmallScreamAudioClips.GetRandom());
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
