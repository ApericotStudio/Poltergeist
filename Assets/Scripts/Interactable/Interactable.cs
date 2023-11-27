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

    public UnityEvent InteractEvent;
    private bool _interactDepleted;

    

    private ObservableObject _observableObject;
    private int _uses = 0;
    public bool InteractDepleted { get => _interactDepleted; set => _interactDepleted = value; }

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
    }

    public void Use()
    {
        if (_uses >= _maxUses && _hasMax)
        {
            return;
        }
        InteractEvent.Invoke();
        ObjectState originalState = _observableObject.State;
        _observableObject.State = ObjectState.Interacted;
        _observableObject.State = originalState;
        _uses++;
        if (_uses >= _maxUses && _hasMax)
        {
            if (gameObject.TryGetComponent(out Highlight highlight)){
                highlight.Highlightable(false);
            }

            _interactDepleted = true;
        }
    }
}
