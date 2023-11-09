using UnityEngine;
using UnityEngine.Events;

public class VisionController : MonoBehaviour
{
    public UnityEvent LookingAtChanged = new UnityEvent();
    public GameObject LookingAt = null;

    [Header("Adjustable Variables")]
    [Tooltip("Maximum distance from camera for an object to be visible")]
    [SerializeField, Range(0f, 30f)] private float _visionRange;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Look();
    }

    private void Look()
    {
        GameObject foundObject = CheckForObject();
        if (foundObject == LookingAt)
        {
            return;
        }
        LookingAt = foundObject;
        LookingAtChanged?.Invoke();
    }

    private GameObject CheckForObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _visionRange))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
