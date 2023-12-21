using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Events;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera _defaultCam;
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
        _possessionController.CurrentPossessionChanged.AddListener(ChangeCameraToPossession);

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
        ExitThrowMode();
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
            ExitThrowMode();
        }
    }

    private void ExitThrowMode()
    {
        throwmode = false;
        ExitThrowModeEvent.Invoke();
    }

    public void ChangeCameraToPossession(GameObject currentPossession)
    {
        if (_possessionController.CurrentPossession == null)
        {
            SwitchCamera(0);
            ExitThrowMode();
            return;
        }
        if (_possessionDefaultCam != null)
        {
            _possessionDefaultCam.Priority = 0;
        }
        ClutterCamera possessionCamScript = _possessionController.CurrentPossession.GetComponent<ClutterCamera>();
        _possessionDefaultCam = possessionCamScript.FollowCam;
        _cameras[1] = _possessionDefaultCam;
        SwitchCamera(1);
    }

    private void SwitchCamera(int index)
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
