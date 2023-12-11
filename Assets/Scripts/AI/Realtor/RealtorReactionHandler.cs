using UnityEngine;
using UnityEngine.UI;

public class RealtorReactionHandler : MonoBehaviour
{
    [Header("Reaction Audio Settings")]
    [Tooltip("The audio clip that will be played when the NPC investigates."), SerializeField]
    private AudioClipList _investigateAudioClips;
    [Tooltip("The audio clip that will be played when the NPC stops investigating."), SerializeField]
    private AudioClipList _investigateEndAudioClips;

    [Header("Reaction Image Settings")]
    [Tooltip("The image that will be used to display the reaction sprite."), SerializeField]
    private Image _reactionImage;
    [Tooltip("The sprite that will be displayed when the NPC is investigating."), SerializeField]
    private Sprite _investigateSprite;

    [Header("Faces")]
    [SerializeField] private Material _restingFace;
    [SerializeField] private Material _investigateFace;
    [SerializeField] private SkinnedMeshRenderer _faceMesh;

    private AiController _aiController;
    private IState _previousState;

    private void Awake()
    {
        _aiController = GetComponent<AiController>();
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

    private void PlayReactionSound()
    {
        AudioClip clip = null;

        switch (_aiController.CurrentState)
        {
            case InvestigateState when _previousState is CheckUpState:
                clip = _investigateAudioClips.GetRandom();
                break;
            case CheckUpState when _previousState is InvestigateState:
                clip = _investigateEndAudioClips.GetRandom();
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
        else
        {
            _reactionImage.enabled = false;
        }
    }
}