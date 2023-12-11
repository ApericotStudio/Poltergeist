using UnityEngine;
using UnityEngine.Events;

public enum Interacter 
{ 
    Player,
    Npc 
}
public class Interactable : MonoBehaviour
{
    [Header("Adjustable variables")]
    [Tooltip("Message displayed on hover")]
    [SerializeField] public string HoverMessage = "Interact";
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool _hasMax = false;
    [SerializeField] private int _maxUses = 10;
    [SerializeField] private bool _scary = true;

    public UnityEvent<Interacter> InteractEvent;
    public UnityEvent MaxUseEvent;
    private bool _interactDepleted;


    private ObservableObject _observableObject;
    private int _uses = 0;
    public bool InteractDepleted { get => _interactDepleted; set => _interactDepleted = value; }

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
    }

    public void Use(Interacter interacter)
    {
        if (_uses >= _maxUses && _hasMax)
        {
            return;
        }
        if(_observableObject.State == ObjectState.Idle && interacter == Interacter.Npc)
        {
            return;
        }
        InteractEvent.Invoke(interacter);

        if(interacter == Interacter.Player)
        {
            if (_scary)
            {
                if(_observableObject.State == ObjectState.Idle)
                    _observableObject.State = ObjectState.Interacted;
                else
                    _observableObject.State = ObjectState.Idle;
            }
            _uses++;
            if (_uses >= _maxUses && _hasMax)
            {
                if (gameObject.TryGetComponent(out Highlight highlight)){
                    highlight.Highlightable(false);
                }

                _interactDepleted = true;
                MaxUseEvent.Invoke();
            }
        }
        else
        {
            _observableObject.State = ObjectState.Idle;
        }
    }
}
