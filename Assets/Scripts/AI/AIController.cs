using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AiController : MonoBehaviour
{
    [Tooltip("The event that will be invoked when the ai changes state.")]
    public delegate void StateChanged(IState state);
    public event StateChanged OnStateChange;

    public NavMeshAgent Agent { get; set; }
    public Animator Animator { get; private set; }

    public int AnimIDMotionSpeed { get; private set; }
    public int AnimIDSpeed { get; private set; }
    
    [HideInInspector]
    public float AnimationBlend;

    private IState _currentState;
    
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
}
