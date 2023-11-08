using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PossessionController : MonoBehaviour
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable currentPossession;
    public GameObject currentPossessionObject { get; private set; }
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

    private void Update()
    {

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
        currentPossession = possessable;
    }

    public void Unpossess()
    {
        if (currentPossession != null)
        {
            currentPossession.Unpossess();
            currentPossession = null;
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
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, possessionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IPossessable possessableObject))
            {
                currentPossessionObject = hit.collider.gameObject;
                return possessableObject;
            }
        }
        return null;
    }
}
