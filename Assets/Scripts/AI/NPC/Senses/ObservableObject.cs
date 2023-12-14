using System;
using System.Collections.Generic;
using System.Globalization;
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
/// The various types of objects that can be in the game.
/// </summary>
public enum ObjectType
{
    Big = 30,
    Medium = 15,
    Small = 10
}

public enum MinimumImpulse
{
    Big = 14,
    Medium = 14,
    Small = 30
}

/// <summary>
/// The Observable Object class is used to store the state of the object. 
/// It also contains the anxiety values that will be added to the NPC's anxiety when the object has entered certain states.
/// </summary>
public class ObservableObject : MonoBehaviour, IObservableObject
{
    [Header("Observable Object Settings")]
    [Tooltip("The type of object.")]
    public ObjectType Type;
    private ObjectState _state = ObjectState.Idle;
    private MinimumImpulse _minimumImpulse;
    public float GeistCharge = 1;

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

    public MinimumImpulse MinimumImpulse
    {
        get => _minimumImpulse;
    }

    private void Awake()
    {
        switch (Type)
        {
            case ObjectType.Big:
                _minimumImpulse = MinimumImpulse.Big;
                break;
            case ObjectType.Medium:
                _minimumImpulse = MinimumImpulse.Medium;
                break;
            case ObjectType.Small:
                _minimumImpulse = MinimumImpulse.Small;
                break;
        }
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
