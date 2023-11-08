using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera _aimCam;
    private CinemachineVirtualCamera _defaultCam;
    private CinemachineVirtualCamera _possessionAimCam;
    private CinemachineVirtualCamera _possessionDefaultCam;
    private CinemachineVirtualCamera[] _cameras;

    private ThirdPersonController _controller;
    private PossessionController _possessionController;

    private Throwable _currentThrowable;

    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 1;
    public bool aimmode;
    // Start is called before the first frame update
    private void Awake()
    {
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[]{ _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentThrowable != null) { _currentThrowable.lineRenderer.enabled = aimmode; }
        if (aimmode)
        {
            if (_currentThrowable == null)
            {
                Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = aimDirection;
            } else
            {
                Vector3 worldAimTarget = _possessionAimCam.transform.position + _possessionAimCam.transform.forward * 1000f;
                worldAimTarget.y = _possessionController.currentPossessionObject.transform.position.y;
                Vector3 aimDirection = (worldAimTarget - _possessionController.currentPossessionObject.transform.position).normalized;
                _possessionController.currentPossessionObject.transform.forward = aimDirection;
                _currentThrowable.DrawProjection();
            }
        }
    }

    public void EnterAimMode()
    {
        aimmode = true;
        if (_possessionController.currentPossessionObject == null)
        {
            _controller.SetSensitivity(aimSensitivity);
            switchCamera(1);
        } else
        {
            _controller.SetSensitivity(aimSensitivity);
            switchCamera(3);
        }
    }
    public void ExitAimModeConfirm()
    {
        if (_possessionController.currentPossessionObject == null)
        {
            _possessionController.Possess();
            if (_possessionController.currentPossessionObject != null)
            {
                changeToPossession();
                switchCamera(2);
                aimmode = false;
                _controller.SetSensitivity(normalSensitivity);
                if (_possessionController.currentPossessionObject.TryGetComponent(out Throwable throwable))
                {
                    _currentThrowable = throwable;
                }
            }
            else
            {
                ExitAimMode();
            }
        }
        else
        {
            if (_currentThrowable != null && aimmode)
            {
                _currentThrowable.Throw();
                ExitAimMode();
            }
        }
    }

    public void ExitAimMode()
    {
        aimmode = false;
        if (_possessionController.currentPossessionObject == null)
        {
            _controller.SetSensitivity(normalSensitivity);
            switchCamera(0);
        } else
        {
            _controller.SetSensitivity(normalSensitivity);
            switchCamera(2);
        }


    }

    private void changeToPossession()
    { 
        Transform pos = _possessionController.currentPossessionObject.transform.Find("ObjectCameraRoot");
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
