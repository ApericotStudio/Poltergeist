using UnityEngine;
using UnityEngine.UI;

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
            OnBeindLookedAtChanged(value);
        }
    }

    private void OnBeindLookedAtChanged(bool value)
    {
        _uiComponent.SetActive(value);
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
