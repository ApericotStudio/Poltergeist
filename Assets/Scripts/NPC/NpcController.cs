using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    [Header("NPC Settings")]
    [Tooltip("The target location the NPC will roam around.")]
    public Transform RoamTargetLocation;
    [Tooltip("The radius around the Roam Target Location the NPC will in."), Range(0f, 10f)]
    public float RoamRadius = 5f;
    [Tooltip("The speed the NPC will move when roaming."), Range(2f, 10f)]
    public float RoamingSpeed = 2f;
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
    [Tooltip("The cooldown on the fear reduction that gets enabled right after an NPC gets scared."), Range(0f, 10f)]
    public float FearReducerCooldown = 5f;
    [Tooltip("The event that will be invoked when the npc changes state.")]
    public UnityEvent OnStateChange;
    [Tooltip("The Game Event Manager that will be used to invoke game events in the various states.")]
    public GameEventManager GameEventManager;

    [Header("NPC Audio Settings")]
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

    [HideInInspector]
    public List<ObservableObject> _usedObjects;
    private INpcState _currentState;
    
    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private float _animationBlend;
    private Animator _animator;

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

    public bool FearReducerHasCooldown{ get; set; }

    public AudioSource NpcAudioSource { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public bool RanAway { get; set; }

    public RoamState RoamState { get; private set; }
    public PanickedState PanickedState { get; private set; }
    public InvestigateState InvestigateState { get; private set; }

    public Transform InvestigateTarget { get; set; }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NpcAudioSource = GetComponent<AudioSource>();
        InitializeAnimator();
        OnStateChange.AddListener(OnStateChanged);
        RoamState = new RoamState(this);
        PanickedState = new PanickedState(this);
        InvestigateState = new InvestigateState(this);
        StartCoroutine(FearReducerCoroutine());
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
        if(CurrentState is not global::RoamState and not global::PanickedState and not global::InvestigateState)
        {
            CurrentState = RoamState;
            return;
        }
    }

    private void OnStateChanged()
    {
        _currentState.Handle();
    }

    IEnumerator FearReducerCoroutine()
    {
        while(true)
        {
            if(FearReducerHasCooldown)
            {
                yield return new WaitForSeconds(FearReducerCooldown);
            }        
            else
            {
                if(FearValue > 0f)
                {
                    FearValue -= 0.15f;
                    yield return new WaitForSeconds(0.05f);

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
