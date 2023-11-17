using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Adjustable variables")]
    [Tooltip("Message displayed on hover")]
    [SerializeField] public string HoverMessage = "Interact";
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool _singleUse = false;

    public UnityEvent InteractEvent;

    private ObservableObject _observableObject;
    private bool _used = false;

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
    }

    public void Use()
    {
        if (_singleUse && _used)
        {
            return;
        }
        InteractEvent.Invoke();
        ObjectState originalState = _observableObject.State;
        _observableObject.State = ObjectState.Interacted;
        _observableObject.State = originalState;
        _used = true;
        if (_singleUse)
        {
            if (gameObject.TryGetComponent(out Highlight highlight)){
                highlight.Highlightable(false);
            }
        }
    }
}
