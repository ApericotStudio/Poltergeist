using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

public class ClutterCamera : MonoBehaviour
{
    [SerializeField] public bool LockCameraPosition = true;
    [SerializeField] public GameObject CinemachineCameraTarget;
    [SerializeField] private float _sensitivity = 1f;
    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] private float _topClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] private float _bottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField] private float _cameraAngleOverride = 0.0f;

    public CinemachineVirtualCamera FollowCam;


    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private bool _isCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Time.deltaTime > 0)
        {
            CameraRotation();
        }
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = _isCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier * _sensitivity;
            _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier * _sensitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    public IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(1.6f);
        _cinemachineTargetYaw = 0;
        _cinemachineTargetPitch = 0;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
