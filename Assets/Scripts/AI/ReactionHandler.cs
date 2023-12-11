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
    private AiController _aiController;

    private void Awake()
    {
        _aiController = GetComponent<AiController>();
        _animator = _reactionImage.GetComponent<Animator>();
        _aiController.OnStateChange.AddListener(OnStateChange);
        _previousState = _aiController.CurrentState;
        
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

        switch (_aiController.CurrentState)
        {
            case InvestigateState when _previousState is RoamState:
                clip = _investigateAudioClips.GetRandom();
                _animator.SetBool("Investigating", true);
                break;
            case InvestigateState when _previousState is CheckUpState:
                clip = _investigateAudioClips.GetRandom();
                _animator.SetBool("Investigating", true);
                break;
            case RoamState when _previousState is InvestigateState:
                clip = _investigateEndAudioClips.GetRandom();
                _animator.SetBool("Investigating", false);
                break;
            case RoamState when _previousState is ScaredState:
                _animator.SetBool("Scared", false);
                break;
            case PanickedState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _terrifiedAudioClips.GetRandom();
                _animator.SetTrigger("Fear");
                break;
            case ScaredState when _previousState is RoamState || _previousState is InvestigateState:
                clip = _bigScreamAudioClips.GetRandom();
                _animator.SetBool("Scared", true);
                break;
            case CheckUpState when _previousState is InvestigateState:
                clip = _investigateEndAudioClips.GetRandom();
                _animator.SetBool("Investigating", false);
                break;
        }

        if (clip != null)
        {
            _aiController.AudioSource.PlayOneShot(clip);
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
}
