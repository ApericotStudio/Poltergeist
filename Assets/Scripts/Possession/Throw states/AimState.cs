using System.Collections;
using UnityEngine;

public class AimState : MonoBehaviour, IThrowState
{
    // references
    private Throwable _controller;
    private LineRenderer _lineRenderer;
    private Camera _camera;
    private Rigidbody _rb;

    // variables
    private Vector3 _aimDirection;

    // don't understand this yet
    [SerializeField][Range(10, 100)] private int _linePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float _timeBetweenPoints = 0.1f;

    public AimState(Throwable controller, LineRenderer lineRenderer, Camera camera, Rigidbody rb)
    {
        _controller = controller;
        _lineRenderer = lineRenderer;
        _camera = camera;
        _rb = rb;
    }

    public void OnStateEnter()
    {
        StartCoroutine(Aim());
        _lineRenderer.enabled = true;
    }

    public void OnStateLeave()
    {
        StopCoroutine(Aim());
        _lineRenderer.enabled = false;
    }

    public void OnAim()
    {
        // do nothing
    }

    public void OnStopAim()
    {
        _controller.SetThrowState(_controller.IdleState);
    }

    public void Throw()
    {
        _controller.GetComponent<Rigidbody>().AddForce(_controller._aim * _controller._throwForce, ForceMode.Impulse);
        _controller.SetThrowState(_controller.ThrownState);
    }

    private IEnumerator Aim()
    {
        while (true)
        {
            _aimDirection = _camera.transform.forward;
            _aimDirection.y = _aimDirection.y * _controller.ySense;
            _aimDirection.Normalize();
            DrawProjection();
            yield return new WaitForEndOfFrame();
        }
    }

    public void DrawProjection()
    {
        Vector3 _releasePosition = _controller.transform.position;
        _lineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _releasePosition;
        Vector3 startVelocity = _controller._throwForce * _aimDirection / _rb.mass;
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

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, _controller._throwLayerMask))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
