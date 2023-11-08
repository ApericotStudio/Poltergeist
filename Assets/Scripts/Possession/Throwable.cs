using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Throwable : MonoBehaviour, IPossessable
{
    [Header("Throw Controls")]
    [SerializeField] private float _throwForce = 15;
    [SerializeField] [Tooltip("Extra sensitivity on y-axis for easier throwing")] private float ySense = 2;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] [Tooltip("Minimum Impulse needed to destroy the object")] private float _destroyMinimumImpulse = 1;
    private Vector3 _releasePosition;

    [Header("Display Controls")]
    [SerializeField] [Range(10, 100)] private int _linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float _timeBetweenPoints = 0.1f;
    private LayerMask _throwLayerMask;

    public bool isPossessed;

    private Camera _cam;
    private Rigidbody _rb;
    private Collider _collider;
    private Vector3 _aim;
    public LineRenderer lineRenderer;
    private Clutter _clutter;
    // Start is called before the first frame update
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        lineRenderer = this.GetComponent<LineRenderer>();
        _cam = Camera.main;
        _clutter = this.GetComponent<Clutter>();
        _collider = this.GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        UpdateClutterState();
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
        _rb.AddForce(_aim * _throwForce, ForceMode.Impulse);
    }

    private void UpdateClutterState()
    {
        if (_clutter.State != ClutterState.Destroyed)
        {
            if (_rb.velocity.magnitude > 0.01f)
            {
                _clutter.State = ClutterState.Moving;
            } else
            {
                _clutter.State = ClutterState.Idle;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > _destroyMinimumImpulse)
        {
            _clutter.State = ClutterState.Destroyed;
        }
    }

    public void DrawProjection()
    {
        _releasePosition = transform.position;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _releasePosition;
        Vector3 startVelocity = _throwForce * _aim / _rb.mass;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _linePoints; time += _timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            //Trajectory formula here
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, _throwLayerMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
