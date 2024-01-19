using UnityEngine;
using UnityEngine.UI;

public class GeistChargeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _uiComponent;
    [SerializeField] private Image _filler;

    // private float _minimumFillerSize = 0.5f;

    [SerializeField] private Vector3 _offset;

    private Transform _playerCam;
    [HideInInspector] public bool BeingLookedAt = false;
    [HideInInspector] public bool IsPossessed;

    private ObservableObject _observableObject;

    private void Awake()
    {
        _observableObject = GetComponentInParent<ObservableObject>();
        _observableObject.OnGeistChargeChanged += OnGeistChargeValueChange;
        _offset = _uiComponent.transform.localPosition;
        _playerCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        _uiComponent.transform.position = transform.parent.position + _offset;
        transform.LookAt(transform.position + _playerCam.forward);
        
        if(_observableObject.State == ObjectState.Broken)
        {
            Destroy(gameObject);
        }
    }

    private void OnGeistChargeValueChange(float value)
    {
        //float desiredScale = Mathf.Lerp(_minimumFillerSize, 1, value);
        //_filler.transform.localScale = new Vector3(desiredScale, desiredScale, 1);

        _filler.fillAmount = value;
        _uiComponent.SetActive(ShouldShowCharge(value));
    }

    private bool ShouldShowCharge(float value)
    {
        return value < 1
        && (IsPossessed || BeingLookedAt
        && _observableObject.State != ObjectState.Broken);
    }

    public void BeforeDestroy()
    {
        _observableObject.OnGeistChargeChanged -= OnGeistChargeValueChange;
        Destroy(_uiComponent);
    }
}
