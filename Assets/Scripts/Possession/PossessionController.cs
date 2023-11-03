using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PossessionController : MonoBehaviour
{
    [Tooltip("Max range for possession")]
    [SerializeField, Range(0f, 20f)] private float possessionRange;

    private IPossessable currentPossession;
    private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera movecam;
    private CinemachineVirtualCamera aimCam;
    private ThirdPersonController controller;

    private bool aimmode;
    [SerializeField] float rotationSpeed = 10;

    private void Start()
    {
        mainCamera = Camera.main;
        aimCam = this.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        controller = this.gameObject.GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (aimmode)
        {
            Look();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            EnterAimMode();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Possess();
            if (currentPossession == null)
            {
                ExitAimCancel();
            }
            else
            {
                ExitAimToPossess();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }
    }

    private void EnterAimMode()
    {
        aimCam.Priority = 1;
        movecam.Priority = 0;
        aimmode = true;
        controller.freeze = true;
    }

    private void ExitAimToPossess()
    {
        aimCam.Priority = 0;
        aimmode = false;
    }

    private void ExitAimCancel()
    {
        movecam.Priority = 1;
        aimCam.Priority = 0;
        aimmode = false;
        controller.freeze = false;
    }

    /// <summary>
    /// If camera is looking at possessable object possess it and unpossess current possession
    /// </summary>
    private void Possess()
    {
        IPossessable possessable = LookForPossessableObject();
        if (possessable == null)
        {
            if (currentPossession != null)
            {
                Unpossess();
            }
            return;
        }
        controller.freeze = true;
        this.movecam.Priority = 0;
        if (this.currentPossession != null)
        {
            currentPossession.Unpossess();
        }
        possessable.Possess();
        currentPossession = possessable;
    }

    private void Unpossess()
    {
        this.movecam.Priority = 1;
        currentPossession.Unpossess();
        currentPossession = null;
        controller.freeze = false;
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
                return possessableObject;
            }
        }
        return null;
    }
    private void Look()
    {
        float playerRotate = rotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, playerRotate, 0);
    }
}
