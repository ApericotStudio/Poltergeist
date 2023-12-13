using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NpcController : AiController
{
    [Header("NPC Settings")]
    [Tooltip("The target location the NPC will run to when frightened.")]
    public Transform FrightenedTargetLocation;
    [Tooltip("The speed the NPC will move when frightened."), Range(2f, 10f)]
    public float FrightenedSpeed = 5.335f;
    [Tooltip("The fear value of the NPC."), SerializeField, Range(0f, 100f)]
    private float _fearValue = 50f;
    [Tooltip("The event that will be invoked when the fear value changes.")]
    public UnityEvent<float> OnFearValueChange;

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
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float FootstepVolume = 0.5f;
    [HideInInspector]
    public bool RanAway;
    [HideInInspector]
    public bool FearReductionHasCooldown = false;
    [HideInInspector]
    public bool SeenByRealtor;
    [HideInInspector]
    public int CurrentRoamIndex = 0;

    public float FearValue
    { 
        get => _fearValue; 
        set {
            _fearValue = value;
            OnFearValueChange.Invoke(_fearValue);
        }  
    }
    public AudioSource NpcAudioSource { get; private set; }
    public RoamState RoamStateInstance { get; private set; }
    public PanickedState PanickedStateInstance { get; private set; }
    public ScaredState ScaredStateInstance { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        NpcAudioSource = GetComponent<AudioSource>();
        InitializeController();
        InitializeStateInstances();
    }
    
    private void FixedUpdate()
    {
        ChangeBehaviourBasedOnAnxiety();
    }

    private void ChangeBehaviourBasedOnAnxiety()
    {
        if(FearValue >= 100f && CurrentState is not PanickedState)
        {
            CurrentState = PanickedStateInstance;
            return;
        }
        if(CurrentState is not RoamState and not PanickedState and not InvestigateState and not ScaredState)
        {
            CurrentState = RoamStateInstance;
            return;
        }
    }

    public void GetScared()
    {
        if(CurrentState is not ScaredState and not PanickedState && FearValue < 100f)
        {
            CurrentState = ScaredStateInstance;
        }
    }    
    
    /// <summary>
    /// Sets the next roam origin. Will loop back to the first origin if the last origin is reached.
    /// </summary>
    public void SetRoamOrigin()
    {
        CurrentRoamIndex = (CurrentRoamIndex + 1) % AvailableRoamOrigins.Length;
        CurrentRoamOrigin = AvailableRoamOrigins[CurrentRoamIndex];
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepVolume);
            }
        }
    }

    private void InitializeStateInstances()
    {
        RoamStateInstance = new RoamState(this);
        PanickedStateInstance = new PanickedState(this);
        InvestigateStateInstance = new InvestigateState(this, RoamStateInstance, CurrentRoamOrigin);
        ScaredStateInstance = new ScaredState(this);
    }
}
