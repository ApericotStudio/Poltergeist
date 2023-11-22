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

    public bool throwmode;

    [SerializeField] private UnityEvent _enterThrowModeEvent;
    [SerializeField] private UnityEvent _exitThrowModeEvent;
    public UnityEvent EnterThrowModeEvent { get => _enterThrowModeEvent; set => _enterThrowModeEvent = value; }
    public UnityEvent ExitThrowModeEvent { get => _exitThrowModeEvent; set => _exitThrowModeEvent = value; }

    private void Awake()
    {
        _controller = this.gameObject.GetComponent<ThirdPersonController>();
        _possessionController = this.gameObject.GetComponent<PossessionController>();
        _possessionController.CurrentPossessionChanged.AddListener(changeCameraToPossession);

        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _possessionDefaultCam };
    }

    private void Update()
    {
        if (_possessionController.CurrentThrowable != null) { _possessionController.CurrentThrowable.LineRenderer.enabled = throwmode; }
        if (throwmode)
        {
            _possessionController.CurrentThrowable.DrawProjection();
        }
    }
    public void CancelThrow()
    {
        _exitThrowModeEvent.Invoke();
        throwmode = false;
    }

    public void PossessConfirm()
    {
        if (throwmode) { return; }
        _possessionController.Possess();
    }

    public void EnterThrowMode()
    {
        if (_possessionController.CurrentThrowable != null)
        {
            if (_possessionController.CurrentThrowable.GetState() is not ObjectState.Idle) { return; }
            _enterThrowModeEvent.Invoke();
            throwmode = true;
        }
    }

    public void ThrowObject()
    {
        if (_possessionController.CurrentThrowable != null && throwmode)
        {
            _possessionController.CurrentThrowable.Throw();
        }
    }

    public void changeCameraToPossession()
    {
        if (_possessionController.CurrentPossession == null)
        {
            switchCamera(0);
            return;
        }
        if (_possessionDefaultCam != null)
        {
            _possessionDefaultCam.Priority = 0;
        }
        ClutterCamera possessionCamScript = _possessionController.CurrentPossession.GetComponent<ClutterCamera>();
        _possessionDefaultCam = possessionCamScript.FollowCam;
        _cameras[1] = _possessionDefaultCam;
        switchCamera(1);
    }

    private void switchCamera(int index)
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            if (_cameras[i] == null) { continue; }
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
