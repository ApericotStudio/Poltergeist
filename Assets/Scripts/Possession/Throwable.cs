using UnityEngine;
using Cinemachine;

public class Throwable : MonoBehaviour, IPossessable
{
    [Header("Throw Controls")]
    [SerializeField] private float _throwForce = 15;
    [SerializeField] [Tooltip("Extra sensitivity on y-axis for easier throwing")] private float ySense = 2;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] [Tooltip("Minimum Impulse needed to destroy the object")] private float _destroyMinimumImpulse = 1;
    [SerializeField]
    [Tooltip("The maximum speed the object is allowed to go before being throwable again")]
    private float _notThrowableThresholdSpeed = 5f;
    private Vector3 _releasePosition;

    [Header("Display Controls")]
    [SerializeField] [Range(10, 100)] private int _linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float _timeBetweenPoints = 0.1f;
    private LayerMask _throwLayerMask;

    public bool isPossessed;

    private Camera _cam;
    private Rigidbody _rb;
    private Vector3 _aim;
    private LineRenderer _lineRenderer;
    private ObservableObject _observableObject;

    public LineRenderer LineRenderer { get => _lineRenderer; set => _lineRenderer = value; }

    // Start is called before the first frame update
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        LineRenderer = this.GetComponent<LineRenderer>();
        _cam = Camera.main;
        _observableObject = this.GetComponent<ObservableObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        _aim = _cam.transform.forward;
        _aim.y = _aim.y * ySense;
        _aim.Normalize();
    }

    public void Possess()
    {
        
        isPossessed = true;
    }

    public void Unpossess()
    {
        isPossessed = false;
    }

    public void Throw()
    {
        
        if(_observableObject.State == ObjectState.Idle)
        {
            _rb.AddForce(_aim * _throwForce, ForceMode.Impulse);
        }        
    }

    public void DrawProjection()
    {
        _releasePosition = transform.position;
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _releasePosition;
        Vector3 startVelocity = _throwForce * _aim / _rb.mass;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _linePoints; time += _timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            //Trajectory formula here
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            LineRenderer.SetPosition(i, point);

            Vector3 lastPosition = LineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, _throwLayerMask))
            {
                LineRenderer.SetPosition(i, hit.point);
                LineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
