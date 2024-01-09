using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GeistChargeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _uiComponent;
    [SerializeField] private Image _filler;

    private Vector3 _offset;
    private Transform _playerCam;

    private bool _beingLookedAt = false;
    public bool BeindLookedAt
    {
        get
        {
            return _beingLookedAt;
        }
        set
        {
            _beingLookedAt = value;
            OnInfoUpdate();
        }
    }

    private bool _isPossessed;
    public bool IsPossessed
    {
        get
        {
            return _isPossessed;
        }
        set
        {
            _isPossessed = value;
            OnInfoUpdate();
        }
    }

    private void OnInfoUpdate()
    {
        bool shouldBeShown = false;

        if (_isPossessed || _beingLookedAt)
        {
            shouldBeShown = true;
        }

        _uiComponent.SetActive(shouldBeShown);
    }

    private void Awake()
    {
        GetComponentInParent<ObservableObject>().OnGeistChargeChanged += OnGeistChargeValueChange;
        _offset = new Vector3(transform.localPosition.x, transform.localPosition.z, transform.localPosition.y);
        _playerCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + _offset;
        transform.LookAt(transform.position + _playerCam.forward);
    }

    private void OnGeistChargeValueChange(float value)
    {
        _filler.fillAmount = value;
    }
}
