using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AiController : MonoBehaviour
{
    [Header("AI Settings")]
    [Tooltip("The speed the AI will move when investigating."), Range(1f, 5f)]
    public float InvestigatingSpeed = 2f;
    [Tooltip("The event that will be invoked when the ai changes state.")]
    public UnityEvent<IState> OnStateChange;

    public NavMeshAgent Agent { get; set; }
    public Animator Animator { get; private set; }
    public Transform InvestigateTarget;

    public int AnimIDMotionSpeed { get; private set; }
    public int AnimIDSpeed { get; private set; }
    public AudioSource AudioSource { get; private set; }
    
    [HideInInspector]
    public float AnimationBlend;

    private IState _currentState;

    public InvestigateState InvestigateState { get; protected set; }
    
    private float lookWeight = 0f;

    public IState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnStateChange.Invoke(_currentState);
        }
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
        if(CurrentState is not global::InvestigateState and not global::PanickedState)
        {
            CurrentState = InvestigateState;
        }
    }

    private void OnAnimatorIK()
    {
        if (CurrentState is InvestigateState)
        {
            if (InvestigateTarget != null)
            {
                lookWeight = Mathf.Lerp(lookWeight, 1f, Time.deltaTime * 2.5f);
                Animator.SetLookAtPosition(InvestigateTarget.position);
            }
            else
            {
                lookWeight = Mathf.Lerp(lookWeight, 0f, Time.deltaTime * 2.5f);
            }
        }
        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0f, Time.deltaTime * 2.5f);
        }
        if(InvestigateTarget != null)
        {
            Animator.SetLookAtPosition(InvestigateTarget.position);
        }
        Animator.SetLookAtWeight(lookWeight);
    }
}
