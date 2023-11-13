using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Events;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera _aimCam;
    private CinemachineVirtualCamera _defaultCam;
    private CinemachineVirtualCamera _possessionAimCam;
    private CinemachineVirtualCamera _possessionDefaultCam;
    private CinemachineVirtualCamera[] _cameras;

    private ThirdPersonController _controller;
    private PossessionController _possessionController;

    [SerializeField]
    private float _normalSensitivity = 1;
    [SerializeField]
    private float _aimSensitivity = 1;
    [SerializeField]
    private float _aimSpeed;

    public bool aimmode;

    [SerializeField] private UnityEvent _enterAimModeEvent;
    [SerializeField] private UnityEvent _exitAimModeEvent;

    public UnityEvent EnterAimModeEvent { get => _enterAimModeEvent; set => _enterAimModeEvent = value; }
    public UnityEvent ExitAimModeEvent { get => _exitAimModeEvent; set => _exitAimModeEvent = value; }

    private void Awake()
    {
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
    }

    private void Update()
    {
        if (_possessionController.currentThrowable != null) { _possessionController.currentThrowable.LineRenderer.enabled = aimmode; }
        if (aimmode)
        {
            if (_possessionController.currentThrowable == null)
            {
                Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = aimDirection;
            }
            else
            {
                _possessionController.currentThrowable.DrawProjection();
            }
        }
    }

    public void EnterAimMode()
    {
        EnterAimModeEvent.Invoke();
        if (_possessionController.currentPossessionObject == null)
        {
            aimmode = true;
            _controller.SetSensitivity(_aimSensitivity);
            switchCamera(1);
        }
        else
        {
            if (_possessionController.currentThrowable != null)
            {
                if (_possessionController.currentThrowable.GetState() is not ObjectState.Idle) { return; }
            }
            aimmode = true;
            _controller.SetSensitivity(_aimSensitivity);
            switchCamera(3);
        }
    }

    public void ExitAimMode()
    {
        ExitAimModeEvent.Invoke();
        aimmode = false;
        if (_possessionController.currentPossessionObject == null)
        {
            _controller.SetSensitivity(_normalSensitivity);
            switchCamera(0);
        }
        else
        {
            _controller.SetSensitivity(_normalSensitivity);
            switchCamera(2);
        }
    }

    public void CancelAimMode()
    {
        if (_possessionController.currentPossessionObject != null && !aimmode)
        {
            _possessionController.Unpossess();
        }
        else
        {
            ExitAimMode();
        }
    }

    public void ConfirmAimMode()
    {
        if (_possessionController.currentPossessionObject == null && aimmode)
        {
            _possessionController.Possess();
        }
        else
        {
            if (_possessionController.currentThrowable != null && aimmode)
            {
                _possessionController.currentThrowable.Throw();
            }
        }
        ExitAimMode();
    }

    public void changeCameraToPossession()
    {
        Transform pos = _possessionController.currentPossessionObject.GetComponent<ClutterCamera>().CinemachineCameraTarget.transform;
        _possessionDefaultCam.LookAt = pos;
        _possessionDefaultCam.Follow = pos;
        _possessionAimCam.LookAt = pos;
        _possessionAimCam.Follow = pos;
        ExitAimMode();
    }

    private void switchCamera(int index)
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            if (i == index)
            {
                _cameras[i].Priority = 1;
            }
            else
            {
                _cameras[i].Priority = 0;
            }
        }

    }
}
