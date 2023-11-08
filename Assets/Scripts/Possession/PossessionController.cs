using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PossessionController : MonoBehaviour, IObserver
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable currentPossession;

    public GameObject currentPossessionObject { get; private set; }
    private ObservableObject _currentObservableObject;
    private Camera mainCamera;
    private ThirdPersonController controller;

    private AimMode aimMode;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        mainCamera = Camera.main;
        aimMode = this.gameObject.GetComponent<AimMode>();
        controller = this.gameObject.GetComponent<ThirdPersonController>();
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
        controller.freeze = true;
        possessable.Possess();
        _currentObservableObject.AddObserver(this);
        currentPossession = possessable;
    }

    public void Unpossess()
    {
        if (currentPossession != null)
        {
            currentPossession.Unpossess();
            aimMode.ExitAimMode();
            currentPossession = null;
            currentPossessionObject = null;
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
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, possessionRange))
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
