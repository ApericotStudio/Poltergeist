using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class AIController : MonoBehaviour
{
    [Tooltip("The event that will be invoked when the ai changes state.")]
    public UnityEvent OnStateChange;

    public NavMeshAgent Agent { get; set; }
    public Animator Animator { get; private set; }

    public int AnimIDMotionSpeed { get; private set; }
    public int AnimIDSpeed { get; private set; }

    public float AnimationBlend;

    private IState _currentState;
    
    public IState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnStateChange.Invoke();
        }
    }
    
    private void OnStateChanged()
    {
        CurrentState.Handle();
    }

    public void InitializeController()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        AnimIDSpeed = Animator.StringToHash("Speed");

        OnStateChange.AddListener(OnStateChanged);
    }
}