using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovementController : MonoBehaviour, IPossessable
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private bool isPossessed = false;

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
        transform.rotation = Quaternion.Euler(new Vector3(0, horizontalInput, 0) * Time.deltaTime * speed + transform.rotation.eulerAngles);
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
