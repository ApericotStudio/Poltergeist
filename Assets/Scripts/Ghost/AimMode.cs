using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera _aimCam;
    private CinemachineVirtualCamera _defaultCam;
    private ThirdPersonController _controller;
    private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 1;
    private bool _aimmode;
    // Start is called before the first frame update
    private void Awake()
    {
        _starterAssetsInputs = this.gameObject.GetComponent<StarterAssetsInputs>();
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (_starterAssetsInputs.aim)
        {
            EnterAimMode();

            Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = aimDirection;
        }
        else
        {
            ExitAimMode();
        }
    }

    public void EnterAimMode()
    {
        _aimmode = true;
        _controller.SetSensitivity(aimSensitivity);
        this._aimCam.Priority = 1;
        this._defaultCam.Priority = 0;
    }

    public void ExitAimMode()
    {
        _aimmode = false;
        _controller.SetSensitivity(normalSensitivity);
        this._aimCam.Priority = 0;
        this._defaultCam.Priority = 1;
    }
}
