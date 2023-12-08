using System;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
    [Header("AI Settings")]
    [Tooltip("The speed the AI will move when investigating."), Range(1f, 5f)]
    public float InvestigatingSpeed = 2f;
    [Tooltip("The event that will be invoked when the ai changes state.")]
    public delegate void StateChanged(IState state);
    public event StateChanged OnStateChange;

    public NavMeshAgent Agent { get; set; }
    public Animator Animator { get; private set; }

    [HideInInspector]
    public Transform InvestigateTarget;

    public int AnimIDMotionSpeed { get; private set; }
    public int AnimIDSpeed { get; private set; }
    
    [HideInInspector]
    public float AnimationBlend;

    private IState _currentState;

    public InvestigateState InvestigateState { get; protected set; }
    
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

        AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        AnimIDSpeed = Animator.StringToHash("Speed");

        OnStateChange += OnStateChanged;
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
                Animator.SetLookAtWeight(1);
                Animator.SetLookAtPosition(InvestigateTarget.position);
            }
            else
            {
                Animator.SetLookAtWeight(0);
            }
        } else
        {
            Animator.SetLookAtWeight(0);
        }

    }
}
