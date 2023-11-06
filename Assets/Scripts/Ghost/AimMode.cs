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

    private ThirdPersonController _controller;
    private PossessionController _possessionController;
    private StarterAssetsInputs _starterAssetsInputs;

    private GameObject _currentPossessionObject;

    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 1;
    private bool _aimmode;
    // Start is called before the first frame update
    private void Awake()
    {
        _starterAssetsInputs = this.gameObject.GetComponent<StarterAssetsInputs>();
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (_aimmode)
        {
            Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = aimDirection;
        }
        if (_starterAssetsInputs.aimButton is Button.isPressed)
        {
            EnterAimMode();
        }
        if (_starterAssetsInputs.aimButton is Button.isReleased)
        {
            _possessionController.Possess();
            ExitAimMode();
            _currentPossessionObject = _possessionController.currentPossessionObject;
            if (_currentPossessionObject != null) {
                changeToPossession();
            }
        }
        if (_starterAssetsInputs.aimCancelButton is Button.isPressed)
        {
            if (!_aimmode)
            {
                _possessionController.Unpossess();
                _currentPossessionObject = null;
            }
            ExitAimMode();
        }
    }

    public void EnterAimMode()
    {
        _aimmode = true;
        if (_currentPossessionObject == null)
        {
            _controller.SetSensitivity(aimSensitivity);
            _aimCam.Priority = 1;
            _defaultCam.Priority = 0;
            _possessionAimCam.Priority = 0;
            _possessionDefaultCam.Priority = 0;
        } else
        {
            _aimCam.Priority = 0;
            _defaultCam.Priority = 0;
            _possessionAimCam.Priority = 1;
            _possessionDefaultCam.Priority = 0;
        }
    }

    public void ExitAimMode()
    {
        _aimmode = false;
        if (_currentPossessionObject == null)
        {
            _controller.SetSensitivity(normalSensitivity);
            _aimCam.Priority = 0;
            _defaultCam.Priority = 1;
            _possessionAimCam.Priority = 0;
            _possessionDefaultCam.Priority = 0;
        } else
        {
            _aimCam.Priority = 0;
            _defaultCam.Priority = 0;
            _possessionAimCam.Priority = 0;
            _possessionDefaultCam.Priority = 1;
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
}
