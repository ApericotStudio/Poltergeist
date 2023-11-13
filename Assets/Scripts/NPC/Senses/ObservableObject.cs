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
/// <summary>
/// The various types of objects that can be in the game.
/// </summary>
public enum ObjectType
{
    Big = 5,
    Small = 2
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
    [Tooltip("The value that gets applied to the NPC's anxiety when the object is audible."), Range(1f, 10f)]
    public float AuditoryAnxietyValue = 1f;
    [Tooltip("The value that gets applied to the NPC's anxiety when the object is visible."), Range(1f, 10f)]
    public float VisualAnxietyValue = 3f;
    [Tooltip("The type of object.")]
    public ObjectType Type;
    [Tooltip("The value that gets added to the NPC's anxiety when the object is small.")]
    public float SmallObjectAnxietyValue = 1f;
    [Tooltip("The value that gets added to the NPC's anxiety when the object is big.")]
    public float BigObjectAnxietyValue = 3f;
    private ObjectState _state = ObjectState.Idle;

    private readonly List<IObserver> _observers = new();
    [HideInInspector]
    public bool IsVisible;
    [HideInInspector]
    public bool IsAudible;

    public ObjectState State 
    { 
        get => _state; 
        set {
            _state = value;
            NotifyObservers();
        }
    }

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
    public void ClearObservers()
    {
        _observers.Clear();
    }
}
