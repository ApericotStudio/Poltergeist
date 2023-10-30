using Cinemachine;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
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

    private void ShowHighlight()
    {
        ThirdPersonCamera thirdPersonCamera = TryGetThirdPersonCamera();
        if (thirdPersonCamera == null)
        {
            return;
        }
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
}
