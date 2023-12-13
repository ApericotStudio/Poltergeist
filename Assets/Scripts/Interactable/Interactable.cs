using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Adjustable variables")]
    [Tooltip("Message displayed on hover")]
    [SerializeField] public string HoverMessage = "Interact";
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool _hasMax = false;
    [SerializeField] private int _maxUses = 10;
    [SerializeField] private bool _scary = true;

    public UnityEvent InteractEvent;
    public UnityEvent MaxUseEvent;
    private bool _interactDepleted;

    [Header("GeistCharge")]
    [SerializeField] private FloatReference _geistChargeDuration;
    private float _geistCharge = 1f;
    private float _outlineMaxSize;
    private bool _geistCharged
    {
        get
        {
            return _geistCharge == 1f;
        }
    }
    private Outline _outline;


    private ObservableObject _observableObject;
    private int _uses = 0;
    public bool InteractDepleted { get => _interactDepleted; set => _interactDepleted = value; }

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
        _outline = GetComponent<Outline>();
        _outlineMaxSize = _outline.OutlineWidth;
    }

    public void Use()
    {
        if (_uses >= _maxUses && _hasMax || !_geistCharged)
        {
            return;
        }
        InteractEvent.Invoke();
        ObjectState originalState = _observableObject.State;
        if (_scary)
        {
            _observableObject.State = ObjectState.Interacted;
        }
        _observableObject.State = originalState;
        _uses++;
        if (_uses >= _maxUses && _hasMax)
        {
            if (gameObject.TryGetComponent(out Highlight highlight)){
                highlight.Highlightable(false);
            }

            _interactDepleted = true;
            MaxUseEvent.Invoke();
        }
        else
        {
            StartCoroutine(RechargeGeist());
        }
    }

    private IEnumerator RechargeGeist()
    {
        _geistCharge = 0;
        _outline.OutlineWidth = 0;
        while (_geistCharge < 1f)
        {
            yield return new WaitForFixedUpdate();
            _geistCharge += Time.deltaTime / _geistChargeDuration.Value;
            if (_geistCharge >= 1f)
            {
                _geistCharge = 1f;
            }
            _outline.OutlineWidth = _geistCharge * _outlineMaxSize;
        }
    }
}
