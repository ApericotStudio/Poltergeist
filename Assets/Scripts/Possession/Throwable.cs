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
    private Vector3 _releasePosition;

    [Header("Display Controls")]
    [SerializeField] [Range(10, 100)] private int _linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float _timeBetweenPoints = 0.1f;
    private LayerMask _throwLayerMask;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public bool _isPossessed;

    private Camera _cam;
    private Rigidbody _rb;
    private Vector3 _aim;
    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _lineRenderer = this.GetComponent<LineRenderer>();
        _cam = Camera.main;

        int throwLayer = this.gameObject.layer;
        for (int i = 0; i < 32; i++)
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
        _aim.y = _aim.y * ySense;
        if (_isPossessed)
        {
            float playerRotate = _rotationSpeed * Input.GetAxis("Mouse X");
            transform.Rotate(0, playerRotate, 0);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                DrawProjection();
            }
            else
            {
                _lineRenderer.enabled = false;
            }


            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ThrowObject();
            }
        }
    }

    public void Possess()
    {
        _virtualCamera.Priority = 1;
        _isPossessed = true;
    }

    public void Unpossess()
    {
        _virtualCamera.Priority = 0;
        _isPossessed = false;
    }

    private void ThrowObject()
    {
        _rb.AddForce(_aim * _throwForce, ForceMode.Impulse);
    }

    private void DrawProjection()
    {
        _releasePosition = transform.position;
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _releasePosition;
        Vector3 startVelocity = _throwForce * _aim / _rb.mass;
        int i = 0;
        _lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _linePoints; time += _timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            //Trajectory formula here
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            _lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = _lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, _throwLayerMask))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
