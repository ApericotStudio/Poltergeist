using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PossessionController : MonoBehaviour, IObserver
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable currentPossession;
    private ObservableObject currentObservableObject;
    
    private Camera mainCamera;
    private CinemachineVirtualCamera virtcam;
    private ThirdPersonController controller;

    private void Start()
    {
        mainCamera = Camera.main;
        virtcam = this.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        controller = this.gameObject.GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }
    }

    /// <summary>
    /// If camera is looking at possessable object possess it and unpossess current possession
    /// </summary>
    private void Possess()
    {
        GameObject possessableObject = LookForPossessableObject();
    
        if (possessableObject == null)
        {
            if (currentPossession != null)
            {
                Unpossess();
            }
            return;
        }
        possessableObject.TryGetComponent(out IPossessable possessable);
        possessableObject.TryGetComponent(out ObservableObject observableObject);

        controller.freeze = true;
        this.virtcam.Priority = 0;
        if (this.currentPossession != null)
        {
            currentPossession.Unpossess();
        }
        possessable.Possess();
        currentPossession = possessable;
        observableObject.AddObserver(this);
    }

    private void Unpossess()
    {
        this.virtcam.Priority = 1;
        currentPossession.Unpossess();
        currentPossession = null;
        controller.freeze = false;
    }

    /// <summary>
    /// Checks if the mainCamera is pointing at an object with the IPossessable interface
    /// </summary>
    /// <returns>The found IPossessable interface or null when no IPossessable interface is found</returns>
    private GameObject LookForPossessableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, possessionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IPossessable possessableObject))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (observableObject.State == ObjectState.Broken)
        {
            Unpossess();
            observableObject.RemoveObserver(this);
        }
    }
}
