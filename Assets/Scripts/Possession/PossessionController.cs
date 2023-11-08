using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PossessionController : MonoBehaviour, IObserver
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable _currentPossession;

    public GameObject currentPossessionObject { get; private set; }
    private ObservableObject _currentObservableObject;
    private Camera _mainCamera;
    private ThirdPersonController _controller;

    private AimMode _aimMode;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _aimMode = this.gameObject.GetComponent<AimMode>();
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
    }

    /// <summary>
    /// If camera is looking at possessable object possess it and unpossess current possession
    /// </summary>
    public void Possess()
    {
        IPossessable possessable = LookForPossessableObject();
    
        if (possessable == null)
        {
            return;
        }
        _controller.freeze = true;
        possessable.Possess();
        _currentObservableObject.AddObserver(this);
        _currentPossession = possessable;
    }

    public void Unpossess()
    {
        if (_currentPossession != null)
        {
            _currentPossession.Unpossess();
            _aimMode.ExitAimMode();
            _currentPossession = null;
            currentPossessionObject = null;
            aimMode.ExitAimMode();
            controller.freeze = false;
        }
    }

    /// <summary>
    /// Checks if the mainCamera is pointing at an object with the IPossessable interface
    /// </summary>
    /// <returns>The found IPossessable interface or null when no IPossessable interface is found</returns>
    private IPossessable LookForPossessableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, possessionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IPossessable possessable))
            {
                currentPossessionObject = hit.collider.gameObject;
                _currentObservableObject = currentPossessionObject.GetComponent<ObservableObject>();
                return possessable;
            }
        }
        return null;
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Broken)
        {
            Unpossess();
        }
    }
}
