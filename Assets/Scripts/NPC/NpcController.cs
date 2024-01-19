using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    [Header("NPC Settings")]
    [Tooltip("The speed the npc will move when investigating."), Range(1f, 5f)]
    public float InvestigatingSpeed = 2f;
    [Tooltip("The time it takes for the NPC to look at a target."), Range(0f, 5f)]
    private float _lookTurningTime = 2f;
    [Tooltip("The event that will be invoked when the npc changes state.")]
    public UnityEvent<IState> OnStateChange;

    public NavMeshAgent Agent { get; set; }
    public Animator Animator { get; private set; }

    public int AnimIDMotionSpeed { get; private set; }
    public int AnimIDSpeed { get; private set; }
    public AudioSource AudioSource { get; private set; }

    public float InvestigateTime { get { return _investigateTime; } }

    [HideInInspector]
    public float AnimationBlend;
    [HideInInspector]
    public Transform InspectTarget;

    private Transform _lookAtTarget = null;

    [SerializeField]
    [Tooltip("Amount of time NPC will look at object before wallking away"), Range(1f, 10f)]
    private float _investigateTime = 3f;

    private IState _currentState;

    public InvestigateState InvestigateStateInstance { get; protected set; }
    [HideInInspector]
    public float LookWeight = 0f;

    public IState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState?.StopStateCoroutines();
            _currentState = value;
            OnStateChange.Invoke(_currentState);
        }
    }

    private void Update()
    {
        Animate();
    }

    private void OnStateChanged(IState state)
    {
        CurrentState.Handle();
    }

    public void InitializeController()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();

        AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        AnimIDSpeed = Animator.StringToHash("Speed");

        OnStateChange.AddListener(OnStateChanged);
    }

    public void Investigate()
    {
        if(CurrentState is not InvestigateState and not PanickedState)
        {
            CurrentState = InvestigateStateInstance;
            LookAt(InspectTarget);
        }
    }

    public IEnumerator UpdateLookWeight(float weight)
    {
        float elapsedTime = 0f;
        while(elapsedTime < _lookTurningTime)
        {
            LookWeight = Mathf.Lerp(LookWeight, weight, elapsedTime / _lookTurningTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnAnimatorIK()
    {
        if(_lookAtTarget != null)
        {
            Animator.SetLookAtPosition(_lookAtTarget.position);
        }
        Animator.SetLookAtWeight(LookWeight, 0.3f, 0.9f, 0, 0);
    }

    public void LookAt(Transform target)
    {
        _lookAtTarget = target;
    }

    private void Animate()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, Agent.velocity.magnitude, Time.deltaTime * Agent.acceleration);
        if (AnimationBlend < 0.01f) AnimationBlend = 0f;

        Animator.SetFloat(AnimIDSpeed, AnimationBlend);
        Animator.SetFloat(AnimIDMotionSpeed, 1f);
    }
}
