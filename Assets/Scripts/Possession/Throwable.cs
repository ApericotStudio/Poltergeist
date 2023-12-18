using System.Collections;
using UnityEngine;

public class Throwable : MonoBehaviour, IPossessable, IObserver
{
    [Header("Throw Controls")]
    [SerializeField] private float _throwForce = 15;
    [SerializeField] [Tooltip("Extra sensitivity on y-axis for easier throwing")] private float ySense = 2;
    [SerializeField] [Tooltip("Aim offset")] private float _aimOffset = 0.5f;
    private Vector3 _releasePosition;

    [Header("Display Controls")]
    [SerializeField] [Range(10, 100)] private int _linePoints;
    [SerializeField] [Range(0.01f, 0.25f)] private float _timeBetweenPoints = 0.1f;
    [SerializeField] private Transform _hitPointImage;
    private LayerMask _throwLayerMask;

    [Header("GeistCharge")]
    [SerializeField] private FloatReference _geistChargeDuration;
    private float _outlineMaxSize;
    private Outline _outline;
    private bool _geistCharged
    {
        get
        {
            return _observableObject.GeistCharge == 1;
        }
    }

    public bool Possessed;

    private Camera _cam;
    private Rigidbody _rb;
    private Vector3 _aim;
    private LineRenderer _lineRenderer { get; set; }
    private ObservableObject _observableObject;
    private ClutterCamera _cameraScript;

    public LineRenderer LineRenderer { get => _lineRenderer; set => _lineRenderer = value; }

    // Start is called before the first frame update
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        LineRenderer = this.GetComponent<LineRenderer>();
        _cam = Camera.main;
        _observableObject = this.GetComponent<ObservableObject>();
        _cameraScript = this.GetComponent<ClutterCamera>();
        _outline = GetComponent<Outline>();
        _outlineMaxSize = _outline.OutlineWidth;
        _observableObject.AddObserver(this);

        int throwLayer = gameObject.layer;
        for(int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(throwLayer, i))
            {
                _throwLayerMask |= 1 << i;
            }
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        _aim = _cam.transform.forward;
        _aim.y = _aim.y * ySense + _aimOffset;
        _aim.Normalize();
    }

    public void Possess()
    {
        _cameraScript.LockCameraPosition = false;
        Possessed = true;
    }

    public void Unpossess()
    {
        StartCoroutine(_cameraScript.ResetCamera());
        _cameraScript.LockCameraPosition = true;
        _lineRenderer.enabled = false;
        Possessed = false;
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
        LineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 2;
        Vector3 startPosition = _releasePosition;
        Vector3 startVelocity = _throwForce * _aim / _rb.mass;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0f; time < _linePoints; time += _timeBetweenPoints)  
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
                _hitPointImage.position = hit.point + hit.normal * 0.01f;
                _hitPointImage.transform.up = hit.normal;                
                return;
            }
        }
        _hitPointImage.position = LineRenderer.GetPosition(i);
    }

    public bool isPossessed()
    {
        return this.Possessed;
    }

    public ObjectState GetState()
    {
        return _observableObject.State;
    }

    private IEnumerator RechargeGeist()
    {
        _observableObject.GeistCharge = 0;
        _outline.OutlineWidth = 0;
        while (_observableObject.GeistCharge < 1f)
        {
            yield return new WaitForFixedUpdate();
            if (_observableObject.State == ObjectState.Broken)
            {
                break;
            }
            _observableObject.GeistCharge += Time.deltaTime / _geistChargeDuration.Value;
            if (_observableObject.GeistCharge >= 1f)
            {
                _observableObject.GeistCharge = 1f;
            }
            _outline.OutlineWidth = _observableObject.GeistCharge * _outlineMaxSize;
        }
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Hit || observableObject.State == ObjectState.Broken)
        {
            if (_geistCharged)
            {
                StartCoroutine(RechargeGeist());
            }
            else
            {
                _observableObject.GeistCharge = 0;
                _outline.OutlineWidth = 0;
            }
        }
    }
}
