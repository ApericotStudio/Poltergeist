using UnityEngine;
using UnityEngine.UI;

public class GeistChargeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _uiComponent;
    [SerializeField] private Image _filler;

    private float _minimumFillerSize = 0.5f;

    [SerializeField] private Vector3 _offset;
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
        _offset = _uiComponent.transform.localPosition;
        _playerCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        _uiComponent.transform.position = transform.parent.position + _offset;
        transform.LookAt(transform.position + _playerCam.forward);
    }

    private void OnGeistChargeValueChange(float value)
    {
        float desiredScale = Mathf.Lerp(_minimumFillerSize, 1, value);
        _filler.transform.localScale = new Vector3(desiredScale, desiredScale, 1);
    }

    public void BeforeDestroy()
    {
        GetComponentInParent<ObservableObject>().OnGeistChargeChanged -= OnGeistChargeValueChange;
        Destroy(_uiComponent);
    }
}