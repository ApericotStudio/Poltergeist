using System.Collections;
using UnityEngine;

public class RealtorController : NpcController
{
    [Header("Realtor Settings")]
    [Tooltip("The speed the realtor will move when roaming."), Range(1f, 5f)]
    public float RoamingSpeed = 2f;

    [Header("Check Up Settings")]
    [Tooltip("The GameObject that contains all the visitors"), SerializeField]
    private GameObject _visitorCollection;
    [Tooltip("The amount of time the realtor will check up on a visitor."), Range(0f, 100f)]
    public float CheckUpTimeSpent = 30f;
    [Tooltip("The radius around the visitor the realtor will roam around in."), Range(1f, 10f)]
    public float CheckUpRadius = 5f;

    [Header("Audio Settings")]
    [Tooltip("The audio clips that will be played when the visitor moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float FootstepVolume = 0.5f;
    [Tooltip("The current check up origin of the realtor. This is the location the realtor will roam around.")]
    public Transform CurrentCheckUpOrigin;

    private int _currentVisitorIndex = 0;

    private CheckUpState _checkUpState;
    private IdleState _idleAfterInvestigateState;
    private SootheState _sootheState;

    private void Awake()
    {
        InitializeController();
        CurrentCheckUpOrigin = _visitorCollection.transform.GetChild(_currentVisitorIndex);
        _checkUpState = new CheckUpState(this);
        _idleAfterInvestigateState = new IdleState(this, _checkUpState, Animator, "Investigate");
        InvestigateStateInstance = new InvestigateState(this, _idleAfterInvestigateState);
        _sootheState = new SootheState(this);
    }

    private void Start() {
        CurrentState = _checkUpState;
    }

    /// <summary>
    /// Sets the check up origin to the next visitor in the visitor collection.
    /// </summary>
    public void SetNextCheckupOrigin()
    {
        _currentVisitorIndex = (_currentVisitorIndex + 1) % _visitorCollection.transform.childCount;
        CurrentCheckUpOrigin = _visitorCollection.transform.GetChild(_currentVisitorIndex);
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                int index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepVolume);
            }
        }
    }

    public void Soothe(VisitorController visitor)
    {
        if (CurrentState is not SootheState)
        {
            _sootheState.Visitor = visitor;
            CurrentState = _sootheState;
        }
    }
}
