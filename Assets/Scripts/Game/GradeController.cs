using System.Collections.Generic;
using UnityEngine;

public class GradeController : MonoBehaviour
{
    [HideInInspector] public Grade Grade;
    private List<ObservableObject> _observableObjectsUsed = new();
    private int _numberOfPhobias;
    private List<ObservableObject> _observableObjectPhobia = new();
    [SerializeField]
    private List<GradeCriteria> _gradeCriterias;
    private void Awake()
    {
        Grade = new Grade();
        Debug.Log(_gradeCriterias.Count);
        switch (_gradeCriterias.Count)
        {
            case 2:
                Grade.GradeObjects = _gradeCriterias[0];
                Grade.GradeTime = _gradeCriterias[1];
                break;
            case 3:
                Grade.GradeObjects = _gradeCriterias[0];
                Grade.GradePhobias = _gradeCriterias[1];
                Grade.GradeTime = _gradeCriterias[2];
                break;
        }

        SetupSubscriptions();
    }

    private void SetupSubscriptions()
    {
        VisitorManager visitorManager = transform.GetComponent<VisitorManager>();
        visitorManager.OnVisitorsLeftChanged += OnVisitorsLeftChanged;
        foreach(FearHandler fearHandler in visitorManager.VisitorCollection.GetComponentsInChildren<FearHandler>())
        {
            fearHandler.OnObjectUsed += OnObjectUsed;
            fearHandler.OnObjectPhobia += OnPhobiaScare;
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
    private void OnPhobiaScare(ObservableObject observableObject)
    {
        if (!_observableObjectPhobia.Contains(observableObject))
        {
            _observableObjectPhobia.Add(observableObject);
            Grade.PhobiaScares++;

            _numberOfPhobias++;
            Debug.Log(_numberOfPhobias);
        }
    }

    private void OnTimePassedChanged(int value)
    {
        Grade.TimePassed = value;
    }
}
