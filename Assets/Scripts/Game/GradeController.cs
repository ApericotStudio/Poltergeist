using System.Collections.Generic;
using UnityEngine;

public class GradeController : MonoBehaviour
{
    [HideInInspector] public Grade Grade;
    private List<ObservableObject> _observableObjectsUsed = new();
    private int _numberOfPhobias;

    private void Awake()
    {
        Grade = new Grade();
        SetupSubscriptions();
    }

    private void SetupSubscriptions()
    {
        VisitorManager visitorManager = transform.GetComponent<VisitorManager>();
        visitorManager.OnVisitorsLeftChanged += OnVisitorsLeftChanged;
        foreach(FearHandler fearHandler in visitorManager.VisitorCollection.GetComponentsInChildren<FearHandler>())
        {
            fearHandler.OnObjectUsed += OnObjectUsed;
            fearHandler.activatedPhobia += OnPhobiaScare;
        }
        transform.GetComponent<GameManager>().OnTimePassedChanged += OnTimePassedChanged;
    }

    private void OnVisitorsLeftChanged(int value)
    {
        Grade.VisitorsLeft = value;
    }

    private void OnObjectUsed(ObservableObject observableObject)
    {
        if (_observableObjectsUsed.Contains(observableObject))
        {
            return;
        }
        _observableObjectsUsed.Add(observableObject);
        Grade.DifferentObjectsUsed++;
    }
    private void OnPhobiaScare(int index)
    {
        Grade.PhobiaScares++;
    }

    private void OnTimePassedChanged(int value)
    {
        Grade.TimePassed = value;
    }
}
