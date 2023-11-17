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
        _possessionController.CurrentPossessionChanged.AddListener(changeCameraToPossession);

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
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
        if(throwmode) { return; }
        if (_possessionController.CurrentPossession == null)
        {
            aimmode = true;
            EnterAimModeEvent.Invoke();
            _controller.SetSensitivity(_aimSensitivity);
            switchCamera(1);
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
            switchCamera(3);
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
        if (_possessionController.CurrentThrowable != null)
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

    public void changeCameraToPossession()
    {
        ExitAimMode();
        if (_possessionController.CurrentPossession == null)
        {
            return;
        }
        Transform pos = _possessionController.CurrentPossession.GetComponent<ClutterCamera>().CinemachineCameraTarget.transform;
        _possessionDefaultCam.LookAt = pos;
        _possessionDefaultCam.Follow = pos;
        _possessionAimCam.LookAt = pos;
        _possessionAimCam.Follow = pos;
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
