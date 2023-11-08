using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    [Header("NPC Settings")]
    [Tooltip("The target location the NPC will roam around.")]
    [SerializeField]
    private Transform _roamTargetLocation;
    [Tooltip("The radius around the Roam Target Location the NPC will in.")]
    [Range(0f, 10f)]
    [SerializeField]
    private float _roamRadius = 5f;
    [Range(2f, 10f)]
    [Tooltip("The speed the NPC will move when roaming.")]
    [SerializeField]
    private float _roamingSpeed = 2f;
    [Tooltip("The target location the NPC will run to when frightened.")]
    [SerializeField]
    private Transform _frightenedTargetLocation;
    [Tooltip("The speed the NPC will move when frightened.")]
    [Range(2f, 10f)]
    [SerializeField]
    private float _frightenedSpeed = 5.335f;
    [Tooltip("The fear value of the NPC.")]
    [SerializeField]
    [Range(0f, 100f)]
    private float _fearValue = 50f;
    [SerializeField]
    [Tooltip("The event that will be invoked when the fear value changes.")]
    private UnityEvent<float> _onFearValueChange;
    [SerializeField]
    [Tooltip("The event that will be invoked when the npc changes state.")]
    private UnityEvent _onStateChange;
    [SerializeField]
    [Tooltip("The Game Event Manager that will be used to invoke game events in the various states.")]
    private GameEventManager _gameEventManager;
    [Header("NPC Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC screams.")]
    [SerializeField]
    private AudioClip[] _screamAudioClips;
    [Tooltip("The volume of the scream audio clips.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _screamVolume = 1f;
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    [SerializeField]
    private AudioClip[] _footstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _footstepVolume = 0.5f;
    
    private NavMeshAgent _navMeshAgent;
    private INpcState _currentState;
    private bool _ranAway;

    private AudioSource _npcAudioSource;
    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private float _animationBlend;
    private Animator _animator;
    
    public UnityEvent OnStateChange { get => _onStateChange; set => _onStateChange = value; }
    public UnityEvent<float> OnFearValueChange { get => _onFearValueChange; set => _onFearValueChange = value; }
    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }
    public bool RanAway { get => _ranAway; set => _ranAway = value; }
    public Transform RoamTargetLocation { get => _roamTargetLocation; set => _roamTargetLocation = value; }
    public float RoamRadius { get => _roamRadius; set => _roamRadius = value; }
    public float RoamingSpeed { get => _roamingSpeed; set => _roamingSpeed = value; }
    public Transform FrightenedTargetLocation { get => _frightenedTargetLocation; set => _frightenedTargetLocation = value; }
    public float FrightenedSpeed { get => _frightenedSpeed; set => _frightenedSpeed = value; }
    public AudioSource NpcAudioSource { get => _npcAudioSource; set => _npcAudioSource = value; }
    
    public AudioClip[] FootstepAudioClips { get => _footstepAudioClips; set => _footstepAudioClips = value; }
    public AudioClip[] ScreamAudioClips { get => _screamAudioClips; set => _screamAudioClips = value; }
    public float FootstepVolume { get => _footstepVolume; set => _footstepVolume = value; }
    public float ScreamVolume { get => _screamVolume; set => _screamVolume = value; }
    public GameEventManager GameEventManager { get => _gameEventManager; set => _gameEventManager = value; }

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
            _onFearValueChange.Invoke(_fearValue);
        }  
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _npcAudioSource = GetComponent<AudioSource>();
        InitializeAnimator();
        OnStateChange.AddListener(OnStateChanged);
    }

    private void Update()
    {
        Animate();
        SlowlyDecreaseAnxiety();
    }

    private void FixedUpdate()
    {
        ChangeBehaviourBasedOnAnxiety();
    }

    private void ChangeBehaviourBasedOnAnxiety()
    {
        if(FearValue <= 0f)
        {
            GameEventManager.OnGameEvent.Invoke(GameEvents.PlayerLost);
            return;
        }
        if(FearValue >= 100f && CurrentState is not PanickedState)
        {
            CurrentState = new PanickedState(this);
            return;
        }
        if(CurrentState is not RoamState && CurrentState is not PanickedState)
        {
            CurrentState = new RoamState(this);
            return;
        }
    }

    private void OnStateChanged()
    {
        _currentState.Handle();
    }

    private void SlowlyDecreaseAnxiety()
    {
        if(FearValue > 0f)
        {
            FearValue -= Time.deltaTime * 1f;
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
        _animationBlend = Mathf.Lerp(_animationBlend, _navMeshAgent.velocity.magnitude, Time.deltaTime * NavMeshAgent.acceleration);
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
                int index = Random.Range(0, _footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.position, _footstepVolume);
            }
        }
    }
}
