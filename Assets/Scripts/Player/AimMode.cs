using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Events;

public class AimMode : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<bool> OnAimModeChange = new UnityEvent<bool>();

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
    private bool _inGhostForm = true;

    [SerializeField] private UnityEvent _enterAimModeEvent;
    [SerializeField] private UnityEvent _exitAimModeEvent;

    public UnityEvent EnterAimModeEvent { get => _enterAimModeEvent; set => _enterAimModeEvent = value; }
    public UnityEvent ExitAimModeEvent { get => _exitAimModeEvent; set => _exitAimModeEvent = value; }

    private void Awake()
    {
        _controller = GetComponent<ThirdPersonController>();
        _possessionController = GetComponent<PossessionController>();
        _possessionController.OnCurrentPossessionChange.AddListener(OnPossessionChanged);

        _aimCam = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        _defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionAimCam = GameObject.Find("PossessionAimCamera").GetComponent<CinemachineVirtualCamera>();
        _possessionDefaultCam = GameObject.Find("PossessionFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
        _cameras = new CinemachineVirtualCamera[] { _defaultCam, _aimCam, _possessionDefaultCam, _possessionAimCam };
    }

    private void Update()
    {
        if (aimmode)
        {
            if (_possessionController.CurrentThrowable == null)
            {
                Vector3 worldAimTarget = _aimCam.transform.position + _aimCam.transform.forward * 1000f;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = aimDirection;
            }
        }
    }

    public void EnterAimMode()
    {
        EnterAimModeEvent.Invoke();
        if (_inGhostForm)
        {
            SetAimMode(true);
            _controller.SetSensitivity(_aimSensitivity);
            SwitchCamera(1);
        }
        else
        {
            SetAimMode(true);
            _controller.SetSensitivity(_aimSensitivity);
            SwitchCamera(3);
        }
    }

    public void ExitAimMode()
    {
        ExitAimModeEvent.Invoke();
        SetAimMode(false);
        if (_inGhostForm)
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
        if (_possessionController.CurrentPossession != null && !aimmode)
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
        if (_possessionController.CurrentPossession == null && aimmode)
        {
            _possessionController.Possess();
        }
        else
        {
            if (_possessionController.CurrentThrowable != null && aimmode)
            {
                _possessionController.CurrentThrowable.Throw();
            }
        }
        ExitAimMode();
    }

    public void ChangeCameraToPossession(GameObject possessedObject)
    {
        ExitAimMode();
        Transform pos = possessedObject.GetComponent<ClutterCamera>().CinemachineCameraTarget.transform;
        _possessionDefaultCam.LookAt = pos;
        _possessionDefaultCam.Follow = pos;
        _possessionAimCam.LookAt = pos;
        _possessionAimCam.Follow = pos;
    }

    private void SetAimMode(bool value)
    {
        bool previousValue = aimmode;
        aimmode = value;
        if (previousValue != value)
        {
            OnAimModeChange?.Invoke(value);
        }
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

    private void OnPossessionChanged(GameObject possessedObject)
    {
        _inGhostForm = possessedObject == null;
        if (!_inGhostForm)
        {
            ChangeCameraToPossession(possessedObject);
            if (possessedObject.TryGetComponent(out Throwable throwable))
            {
                throwable.SetAimMode(this);
            }
        }
    }
}
