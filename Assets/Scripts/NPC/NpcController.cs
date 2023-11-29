using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    [Header("NPC Settings")]
    [Tooltip("The speed the NPC will move when investigating."), Range(1f, 5f)]
    public float InvestigatingSpeed = 2f;
    [Tooltip("The target location the NPC will run to when frightened.")]
    public Transform FrightenedTargetLocation;
    [Tooltip("The speed the NPC will move when frightened."), Range(2f, 10f)]
    public float FrightenedSpeed = 5.335f;
    [Tooltip("The fear value of the NPC."), SerializeField, Range(0f, 100f)]
    private float _fearValue = 50f;
    [Tooltip("The event that will be invoked when the fear value changes.")]
    public UnityEvent<float> OnFearValueChange;
    [Tooltip("The event that will be invoked when the npc changes state.")]
    public UnityEvent OnStateChange;

    [Header("Roaming Settings")]
    [Tooltip("The current roam origin of the NPC. This is the location the NPC will roam around.")]
    public Transform CurrentRoamOrigin;
    [Tooltip("The available roam origins of the NPC. The NPC will loop through these locations when roaming.")]
    public Transform[] AvailableRoamOrigins;
    [Tooltip("The radius around the origin the NPC will roam around in."), Range(1f, 10f)]
    public float RoamRadius = 5f;
    [Tooltip("The speed the NPC will move when roaming."), Range(2f, 10f)]
    public float RoamingSpeed = 2f;
    [Tooltip("The amount of time the NPC will stay around one roam origin"), Range(0f, 100f)]
    public float RoamOriginTimeSpent = 50f;
    [Header("Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC screams.")]
    public AudioClip[] ScreamAudioClips;
    [Tooltip("The audio clip that will be played when the NPC investigates.")]
    public AudioClip InvestigateAudioClip;
    [Tooltip("The audio clip that will be played when the NPC stops investigating.")]
    public AudioClip InvestigateEndAudioClip;
    [Tooltip("The volume of the scream audio clips.")]
    [Range(0f, 1f)]
    public float ScreamVolume = 1f;
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float FootstepVolume = 0.5f;

    [Header("Fear Reduction Settings")]
    [Tooltip("The value that will be subtracted from the fear value."), Range(0.1f, 1f)]
    public float ReductionValue = 0.1f;
    [Tooltip("The speed at which the fear value will be reduced."), Range(0.01f, 1f)]
    public float ReductionSpeed = 0.05f;

    [HideInInspector]
    public List<ObservableObject> _usedObjects;
    [HideInInspector]
    public Transform InvestigateTarget;
    [HideInInspector]
    public bool FearReductionHasCooldown;
    [HideInInspector]
    public bool RanAway;

    private INpcState _currentState;
    
    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private float _animationBlend;
    private Animator _animator;
    private int _currentRoamIndex = 0;
    
    public INpcState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnStateChange.Invoke();
        }
    }

    public float FearValue
    { 
        get => _fearValue; 
        set {
            _fearValue = value;
            OnFearValueChange.Invoke(_fearValue);
        }  
    }
    public AudioSource NpcAudioSource { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public RoamState RoamState { get; private set; }
    public PanickedState PanickedState { get; private set; }
    public InvestigateState InvestigateState { get; private set; }
    public ScaredState ScaredState { get; private set; }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NpcAudioSource = GetComponent<AudioSource>();
        InitializeAnimator();
        OnStateChange.AddListener(OnStateChanged);
        RoamState = new RoamState(this);
        PanickedState = new PanickedState(this);
        InvestigateState = new InvestigateState(this);
        ScaredState = new ScaredState(this);
        StartCoroutine(FearReductionCoroutine());
    }

    private void Update()
    {
        Animate();
    }

    private void FixedUpdate()
    {
        ChangeBehaviourBasedOnAnxiety();
    }

    private void ChangeBehaviourBasedOnAnxiety()
    {
        if(FearValue >= 100f && CurrentState is not global::PanickedState)
        {
            CurrentState = PanickedState;
            return;
        }
        if(CurrentState is not global::RoamState and not global::PanickedState and not global::InvestigateState and not global::ScaredState)
        {
            CurrentState = RoamState;
            return;
        }
    }

    private void OnStateChanged()
    {
        _currentState.Handle();
    }

    IEnumerator FearReductionCoroutine()
    {
        while(true)
        {
            if(CurrentState is PanickedState || FearValue <= 0f)
            {
                yield break;
            }
            if(FearReductionHasCooldown)
            {
                yield return null;
            } 
            else 
            {
                if(FearValue > 0f)
                {
                    FearValue -= ReductionValue;
                    yield return new WaitForSeconds(ReductionSpeed);
                }  
            }
        }
    }

    private void InitializeAnimator()
    {
        _animator = GetComponent<Animator>();
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDSpeed = Animator.StringToHash("Speed");
    }

    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, NavMeshAgent.velocity.magnitude, Time.deltaTime * NavMeshAgent.acceleration);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

     /// <summary>
    /// Sets the next roam origin. Will loop back to the first origin if the last origin is reached.
    /// </summary>
    public void SetRoamOrigin()
    {
        _currentRoamIndex = (_currentRoamIndex + 1) % AvailableRoamOrigins.Length;
        CurrentRoamOrigin = AvailableRoamOrigins[_currentRoamIndex];
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
}
