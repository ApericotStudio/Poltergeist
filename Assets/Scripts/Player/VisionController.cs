using UnityEngine;
using UnityEngine.Events;

public class VisionController : MonoBehaviour
{
    public UnityEvent LookingAtChanged = new UnityEvent();
    public GameObject LookingAt = null;

    [Header("Adjustable Variables")]
    [Tooltip("Maximum distance from camera for an object to be visible")]
    [SerializeField, Range(0f, 30f)] private float _visionRange;

    [Tooltip("The layers that block the player's vision")]
    [SerializeField]
    private LayerMask _obstacleLayerMask;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Look();
    }

    /// <summary>
    /// Updates LookingAt when player looks at a different game object
    /// </summary>
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

    public void ResetLook()
    {
        LookingAt = null;
    }

    /// <summary>
    /// Check if the middle of the camera is pointing at a game object that is no more than _visionRange away
    /// </summary>
    /// <returns></returns>
    private GameObject CheckForObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _visionRange, _obstacleLayerMask))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
