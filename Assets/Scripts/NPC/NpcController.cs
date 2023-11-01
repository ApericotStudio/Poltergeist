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
    [Tooltip("The anxiety value of the NPC.")]
    [SerializeField]
    [Range(0f, 100f)]
    private float _anxietyValue = 50f;
    [SerializeField]
    [Tooltip("The event that will be invoked when the anxiety value changes.")]
    private UnityEvent<float> _onAnxietyValueChange;

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

    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private float _animationBlend;
    private Animator _animator;

    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }
    public INpcState CurrentState { get => _currentState; set => _currentState = value; }
    public bool RanAway { get => _ranAway; set => _ranAway = value; }
    public Transform RoamTargetLocation { get => _roamTargetLocation; set => _roamTargetLocation = value; }
    public float RoamRadius { get => _roamRadius; set => _roamRadius = value; }
    public float RoamingSpeed { get => _roamingSpeed; set => _roamingSpeed = value; }
    public Transform FrightenedTargetLocation { get => _frightenedTargetLocation; set => _frightenedTargetLocation = value; }
    public float FrightenedSpeed { get => _frightenedSpeed; set => _frightenedSpeed = value; }
    public AudioClip[] FootstepAudioClips { get => _footstepAudioClips; set => _footstepAudioClips = value; }
    public float FootstepVolume { get => _footstepVolume; set => _footstepVolume = value; }

    public float AnxietyValue
    { 
        get => _anxietyValue; 
        set {
            _anxietyValue = value;
            _onAnxietyValueChange.Invoke(_anxietyValue);
        }  
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        InitializeAnimator();
        _currentState = new RoamState(this);
    }

    private void Update()
    {
        Animate();
        _currentState.Execute();
        SlowlyDecreaseAnxiety();
    }

    private void SlowlyDecreaseAnxiety()
    {
        if(AnxietyValue > 0f)
        {
            AnxietyValue -= Time.deltaTime * 1f;
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
