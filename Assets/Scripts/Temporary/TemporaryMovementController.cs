using Cinemachine;
using UnityEngine;

public class TemporaryMovementController : MonoBehaviour, IPossessable
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private bool isPossessed = false;
    [SerializeField] float rotationSpeed = 10;

    private float horizontalInput;
    private float speed = 25;

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (isPossessed)
        {
            Look();
        }
    }

    private void Look()
    {
        float playerRotate = rotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, playerRotate, 0);
    }

    public void Possess()
    {
        virtualCamera.Priority = 1;
        isPossessed = true;
    }

    public void Unpossess()
    {
        virtualCamera.Priority = 0;
        isPossessed = false;
    }
}
