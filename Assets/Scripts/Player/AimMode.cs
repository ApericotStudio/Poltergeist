using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Events;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera _aimCam;
    private CinemachineVirtualCamera _defaultCam;
    private CinemachineVirtualCamera _possessionAimCam;
    private CinemachineVirtualCamera _possessionAimCam1;
    private CinemachineVirtualCamera _possessionDefaultCam;
    private CinemachineVirtualCamera _possessionDefaultCam1;
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
    public bool throwmode;

    [SerializeField] private UnityEvent _enterAimModeEvent;
    [SerializeField] private UnityEvent _exitAimModeEvent;
    [SerializeField] private UnityEvent _enterThrowModeEvent;
    [SerializeField] private UnityEvent _exitThrowModeEvent;

    public UnityEvent EnterAimModeEvent { get => _enterAimModeEvent; set => _enterAimModeEvent = value; }
    public UnityEvent ExitAimModeEvent { get => _exitAimModeEvent; set => _exitAimModeEvent = value; }
    public UnityEvent EnterThrowModeEvent { get => _enterAimModeEvent; set => _enterAimModeEvent = value; }
    public UnityEvent ExitThrowModeEvent { get => _exitAimModeEvent; set => _exitAimModeEvent = value; }

    private void Awake()
    {
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();
        _possessionController.CurrentPossessionChanged.AddListener(ChangeCameraToPossession);

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam1 = GameObject.Find("PossessionAimCamera1").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam1 = GameObject.Find("PossessionFollowCamera1").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
    }

    private void Update()
    {
        if (_possessionController.CurrentThrowable != null) { _possessionController.CurrentThrowable.LineRenderer.enabled = throwmode; }
        if (aimmode)
        {
            if (_possessionController.CurrentThrowable == null)
            {
                Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = aimDirection;
            }
            else if (throwmode)
            {
                _possessionController.CurrentThrowable.DrawProjection();
            }
        }
    }

    public void EnterAimMode()
    {
        if(throwmode || aimmode) { return; }
        if (_possessionController.CurrentPossession == null)
        {
            aimmode = true;
            EnterAimModeEvent.Invoke();
            _controller.SetSensitivity(_aimSensitivity);
            SwitchCamera(1);
        }
        else
        {
            if (_possessionController.CurrentThrowable != null)
            {
                if (_possessionController.CurrentThrowable.GetState() is not ObjectState.Idle) { return; }
            }
            aimmode = true;
            EnterAimModeEvent.Invoke();
            _controller.SetSensitivity(_aimSensitivity);
            SwitchCamera(3);
        }
    }

    public void ExitAimMode()
    {
        ExitAimModeEvent.Invoke();
        if (throwmode) { _exitThrowModeEvent.Invoke(); }
        aimmode = false;
        throwmode = false;
        if (_possessionController.CurrentPossession == null)
        {
            _controller.SetSensitivity(_normalSensitivity);
            SwitchCamera(0);
        }
        else
        {
            _controller.SetSensitivity(_normalSensitivity);
            SwitchCamera(2);
        }
    }

    public void CancelAimMode()
    {
        ExitAimMode();
    }

    public void ConfirmPossessAimMode()
    {
        if (throwmode) { return; }
        if (aimmode)
        {
            _possessionController.Possess();
        }
        ExitAimMode();
    }

    public void EnterThrowMode()
    {
        if (_possessionController.CurrentThrowable != null && (!throwmode && !aimmode))
        {
            EnterAimMode();
            _enterThrowModeEvent.Invoke();
            throwmode = true;
        }
    }

    public void ThrowObject()
    {
        if (_possessionController.CurrentThrowable != null && throwmode)
        {
            _possessionController.CurrentThrowable.Throw();
            ExitAimMode();
        }
    }

    /// <summary>
    /// When not possessing only does exitaimmode. When possessing, sets new follow camera and aim camera to the other object, then
    /// swaps the possession cameras in the camera array with the new cameras. ExitAimMode then raises the priority of the new cameras.
    /// Raising priority now causes a transition from old possession camera to new possession camera, which is a smooth transition.
    /// </summary>
    public void ChangeCameraToPossession()
    {
        if (_possessionController.CurrentPossession == null)
        {
            ExitAimMode();
            return;
        }
        Transform newCameraPosition = _possessionController.CurrentPossession.GetComponent<ClutterCamera>().CinemachineCameraTarget.transform;
        CinemachineVirtualCamera newDefaultCamera;
        CinemachineVirtualCamera newAimCamera;
        if (_possessionDefaultCam == _cameras[2])
        {
            newDefaultCamera = _possessionDefaultCam1;
            newAimCamera = _possessionAimCam1;

        } else
        {
            newDefaultCamera = _possessionDefaultCam;
            newAimCamera = _possessionAimCam;
        }
        newDefaultCamera.LookAt = newCameraPosition;
        newDefaultCamera.Follow = newCameraPosition;
        newAimCamera.LookAt = newCameraPosition;
        newAimCamera.Follow = newCameraPosition;
        _cameras[2].Priority = 0;
        _cameras[3].Priority = 0;
        _cameras[2] = newDefaultCamera;
        _cameras[3] = newAimCamera;
        ExitAimMode();
    }

    private void SwitchCamera(int index)
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
