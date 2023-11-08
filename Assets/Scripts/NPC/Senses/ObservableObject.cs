using System;
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
    Broken
}
public enum ObjectType
{
    Big,
    Small
}
/// <summary>
/// The Observable Object class is used to store the state of the object. 
/// It also contains the anxiety values that will be added to the NPC's anxiety when the object has entered certain states.
/// </summary>
public class ObservableObject : MonoBehaviour, IObservableObject
{
    [Header("Observable Object Settings")]
    [Tooltip("This value gets added to the NPC's anxiety when the object moves.")]
    public float MoveAnxietyValue = 2f;
    [Tooltip("This value gets added to the NPC's anxiety when the object is destroyed.")]
    public float DestroyAnxietyValue = 10f;
    [Tooltip("The value that gets applied to the NPC's anxiety when the object is audible.")]
    [Range(1f, 10f)]
    public float AuditoryAnxietyValue = 1f;
    [Tooltip("The value that gets applied to the NPC's anxiety when the object is visible.")]
    [Range(1f, 10f)]
    public float VisualAnxietyValue = 3f;
    [SerializeField]
    [Tooltip("The type of object.")]
    private ObjectType _objectType;
    [SerializeField]
    [Tooltip("The value that gets added to the NPC's anxiety when the object is small.")]
    private float _smallObjectAnxietyValue = 1f;
    [SerializeField]
    [Tooltip("The value that gets added to the NPC's anxiety when the object is big.")]
    private float _bigObjectAnxietyValue = 3f;
    [SerializeField]
    private ObjectState _state = ObjectState.Idle;

    private readonly List<IObserver> _observers = new();

    private bool _isAudible = false;
    private bool _isVisible = false;

    public bool IsVisible { get => _isVisible; set => _isVisible = value; }
    public bool IsAudible { get => _isAudible; set => _isAudible = value; }

    public ObjectState State 
    { 
        get => _state; 
        set {
            _state = value;
            NotifyObservers();
        }
    }
    public ObjectType Type { get => _objectType; set => _objectType = value; }
    public float SmallObjectAnxietyValue { get => _smallObjectAnxietyValue; set => _smallObjectAnxietyValue = value; }
    public float BigObjectAnxietyValue { get => _bigObjectAnxietyValue; set => _bigObjectAnxietyValue = value; }

    public void NotifyObservers()
    {
        foreach (IObserver observer in _observers)
        {
            observer.OnNotify(this);
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
}
