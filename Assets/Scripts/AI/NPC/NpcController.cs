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
    [Tooltip("Makes the NPC more scared for specific items based on the phobia")]
    public ObjectPhobia NPCPhobia;

    [Header("Roaming Settings")]
    [Tooltip("The current room the NPC is in.")]
    public Room CurrentRoom;
    [Tooltip("The available roam origins of the NPC. The NPC will loop through these locations when roaming.")]
    public Room[] AvailableRooms;
    [Tooltip("The radius around the origin the NPC will roam around in."), Range(1f, 10f)]
    public float RoamRadius = 5f;
    [Tooltip("The speed the NPC will move when roaming."), Range(2f, 10f)]
    public float RoamingSpeed = 2f;
    [Tooltip("The amount of time the NPC will stay in one room."), Range(0f, 100f)]
    public float RoomTimeSpent = 50f;

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
    public int CurrentRoomIndex = 0;

    public float FearValue
    { 
        get => _fearValue; 
        set {
            _fearValue = value;
            OnFearValueChange.Invoke(_fearValue);
        }  
    }
    public AudioSource NpcAudioSource { get; private set; }
    public IdleState IdleStateInstance { get; private set; }
    public RoamState RoamStateInstance { get; private set; }
    public PanickedState PanickedStateInstance { get; private set; }
    public ScaredState ScaredStateInstance { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
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
        if(CurrentState is null)
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
    /// Switches to the next room. Will loop back to the first room if the last room is reached.
    /// </summary>
    public void SwitchRooms()
    {
        CurrentRoomIndex = (CurrentRoomIndex + 1) % AvailableRooms.Length;
        CurrentRoom = AvailableRooms[CurrentRoomIndex];
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
        InvestigateStateInstance = new InvestigateState(this, RoamStateInstance);
        ScaredStateInstance = new ScaredState(this);
        IdleStateInstance = new IdleState(this);
    }
}
