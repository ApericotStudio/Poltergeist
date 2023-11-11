using UnityEngine;

public class Throwable : MonoBehaviour, IPossessable
{
    [Header("Throw Controls")]
    public float _throwForce = 15;
    [Tooltip("Extra sensitivity on y-axis for easier throwing")] public float ySense = 2;
    [SerializeField] [Tooltip("Minimum Impulse needed to destroy the object")] private float _destroyMinimumImpulse = 1;

    [Header("Display Controls")]
    public LayerMask _throwLayerMask;

    public bool isPossessed;

    private Camera _cam;
    private Rigidbody _rb;
    public Vector3 _aim { get; private set; }
    private LineRenderer _lineRenderer { get; set; }
    private ObservableObject _observableObject;

    public LineRenderer LineRenderer { get => _lineRenderer; set => _lineRenderer = value; }

    private IThrowState _throwState;
    public IdleState IdleState { get; private set; }
    public AimState AimState { get; private set; }
    public ThrownState ThrownState { get; private set; }

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        LineRenderer = this.GetComponent<LineRenderer>();
        _cam = Camera.main;
        _observableObject = this.GetComponent<ObservableObject>();

        IdleState = new IdleState(this);
        AimState = new AimState(this, LineRenderer, _cam, _rb);
        ThrownState = new ThrownState(this);
        _throwState = IdleState;
    }

    public void SetThrowState(IThrowState state)
    {
        _throwState.OnStateLeave();
        _throwState = state;
        _throwState.OnStateEnter();
    }

    public void Possess()
    {
        isPossessed = true;
    }

    public void Unpossess()
    {
        _lineRenderer.enabled = false;
        isPossessed = false;
    }

    public void Throw()
    {
        if(_observableObject.State == ObjectState.Idle)
        {
            _rb.AddForce(_aim * _throwForce, ForceMode.Impulse);
        }        
    }

    public ObjectState GetState()
    {
        return _observableObject.State;
    }

    private void OnAimCancelled()
    {
        _throwState.OnStopAim();
    }

    private void OnAimStateChanged(bool isAiming)
    {
        if (isAiming)
        {
            _throwState.OnAim();
        }
        else
        {
            _throwState.Throw();
        }
    }
}
