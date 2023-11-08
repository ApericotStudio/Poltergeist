using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservableObject
{
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}
