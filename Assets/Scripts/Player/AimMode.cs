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
    private CinemachineVirtualCamera[] cameras;

    private ThirdPersonController _controller;
    private PossessionController _possessionController;

    private GameObject _currentPossessionObject;
    private Throwable _currentThrowable;

    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 1;
    private bool _aimmode;
    // Start is called before the first frame update
    private void Awake()
    {
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cameras = new CinemachineVirtualCamera[]{ _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentThrowable != null) { _currentThrowable.lineRenderer.enabled = _aimmode; }
        if (_aimmode)
        {
            Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = aimDirection;
            if (_currentThrowable != null)
            {
                _currentThrowable.DrawProjection();
            }
        }
    }

    public void EnterAimMode()
    {
        _aimmode = true;
        if (_currentPossessionObject == null)
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
        if (_currentPossessionObject == null)
        {
            _possessionController.Possess();
            _currentPossessionObject = _possessionController.currentPossessionObject;
            if (_currentPossessionObject != null)
            {
                changeToPossession();
                switchCamera(2);
                _aimmode = false;
                _controller.SetSensitivity(normalSensitivity);
                if (_currentPossessionObject.TryGetComponent(out Throwable throwable))
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
            if (_currentThrowable != null && _aimmode)
            {
                _currentThrowable.Throw();
                ExitAimMode();
            }
        }
    }

    public void ExitAimMode()
    {
        if (!_aimmode && _currentPossessionObject != null)
        {
            _possessionController.Unpossess();
            _currentPossessionObject = null;
            _currentThrowable = null;
        }
        _aimmode = false;
        if (_currentPossessionObject == null)
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
        GameObject currentPossession = _possessionController.currentPossessionObject;
        Transform pos = currentPossession.transform;
        _possessionDefaultCam.LookAt = pos;
        _possessionDefaultCam.Follow = pos;
        _possessionAimCam.LookAt = pos;
        _possessionAimCam.Follow = pos;
        ExitAimMode();
    }

    private void switchCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == index)
            {
                cameras[i].Priority = 1;
            }
            else
            {
                cameras[i].Priority = 0;
            }
        }

    }
}
