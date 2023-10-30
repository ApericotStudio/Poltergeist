using Cinemachine;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
    private Highlight target;
    private CinemachineVirtualCamera currentVirtualCamera;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        currentVirtualCamera = (CinemachineVirtualCamera) mainCamera.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }
    }

    private void FixedUpdate()
    {
        LookForPossessables();
    }

    public void Possess()
    {
        ThirdPersonCamera thirdPersonCamera = TryGetThirdPersonCamera();
        if (thirdPersonCamera == null)
        {
            return;
        }
        currentVirtualCamera.Priority = 0;
        thirdPersonCamera.VirtualCamera.Priority = 1;
        currentVirtualCamera = thirdPersonCamera.VirtualCamera;
    }

    public ThirdPersonCamera TryGetThirdPersonCamera()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent(out ThirdPersonCamera thirdPersonCamera))
            {
                return thirdPersonCamera;
            }
        }
        return null;
    }

    private void LookForPossessables()
    {
        Highlight temp = null;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent(out Highlight highlight))
            {
                temp = highlight;
            }
        }
        if (temp == target)
        {
            return;
        }
        if (target != null)
        {
            target.ToggleHighlight(false);
        }
        target = temp;
        if (target != null)
        {
            target.ToggleHighlight(true);
        }
    }
}
