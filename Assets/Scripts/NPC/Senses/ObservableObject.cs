using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The various states that the object can be in.
/// </summary>
public enum ObjectState
{
    Idle,
    Moving,
    Hit,
    Broken,
    Interacted
}

/// <summary>
/// The various phobias an object can have
/// </summary>
public enum ObjectPhobia
{
    Loud,
    Dark,
    Technology,
    None
}

/// <summary>
/// The Observable Object class is used to store the state of the object. 
/// It also contains the anxiety values that will be added to the NPC's anxiety when the object has entered certain states.
/// </summary>
public class ObservableObject : MonoBehaviour, IObservableObject
{
    [Header("Observable Object Settings")]
    [Tooltip("The base fear value of this object")]
    [SerializeField]
    public FloatReference SizeFear;
    private ObjectState _state = ObjectState.Idle;
    [Tooltip("The minimum impulse for this object to cause a scare")]
    [SerializeField]
    private FloatReference _minimumImpulse;
    [SerializeField]
    private List<ObjectPhobia> _objectPhobia = new List<ObjectPhobia>();
    [HideInInspector] public float GeistCharge = 1;

    private readonly List<IObserver> _observers = new();

    public ObjectState State 
    { 
        get => _state; 
        set {
            if(_state != value)
            {
                _state = value;
                NotifyObservers();
            }
        }
    }

    public float MinimumImpulse
    {
        get => _minimumImpulse.Value;
    }
    public List<ObjectPhobia> ObjectPhobia
    {
        get => _objectPhobia;
    }

    private void Awake()
    {
        NotifyObservers();
    }
    public void NotifyObservers()
    {
        for (int i = 0; i < _observers.Count; i++)
        {
            _observers[i].OnNotify(this);
        }
    }

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    public void ClearObservers()
    {
        _observers.Clear();
    }
}
