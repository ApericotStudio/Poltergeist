using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    Roam,
    Frightened
}
public class NpcController : MonoBehaviour
{
    [Header("NPC Settings")]
    [Tooltip("The target location the NPC will roam around.")]
    public Transform RoamTargetLocation;
    [Tooltip("The radius around the Roam Target Location the NPC will in.")]
    [Range(0f, 10f)]
    public float RoamRadius = 5f;
    [Range(2f, 10f)]
    [Tooltip("The speed the NPC will move when roaming.")]
    public float RoamingSpeed = 2f;
    [Tooltip("The target location the NPC will run to when frightened.")]
    public Transform FrightenedTargetLocation;
    [Tooltip("The speed the NPC will move when frightened.")]
    [Range(2f, 10f)]
    public float FrightenedSpeed = 5.335f;
    [Tooltip("Enable this to make the NPC run away.")]
    public bool IsFrightened;
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float footstepVolume = 0.5f;

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

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        InitializeAnimator();
        _currentState = new RoamState(this);
    }

    private void Update()
    {
        if(IsFrightened && _currentState.GetType() != typeof(FrightenedState))
        {
            TurnFrightened();
        }
        Animate();
        _currentState.Execute();

    }

    private void TurnFrightened()
    {
        _currentState = new FrightenedState(this);
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
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, footstepVolume);
            }
        }
    }
}
