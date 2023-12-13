using System.Collections.Generic;
using UnityEngine;

public class GradeController : MonoBehaviour
{
    [HideInInspector] public Grade Grade;
    private List<ObservableObject> _observableObjectsUsed = new List<ObservableObject>();

    private void Awake()
    {
        Grade = new Grade();
        SetupSubscriptions();
    }

    private void SetupSubscriptions()
    {
        NpcManager npcManager = transform.GetComponent<NpcManager>();
        npcManager.OnNpcsLeftChanged += OnNpcsLeftChanged;
        foreach(FearHandler fearHandler in npcManager.NpcCollection.GetComponentsInChildren<FearHandler>())
        {
            fearHandler.OnObjectUsed += OnObjectUsed;
        }
        transform.GetComponent<GameManager>().OnTimeLeftChanged += OnTimeLeftChanged;
    }

    private void OnNpcsLeftChanged(int value)
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

    private void OnPhobiaScare()
    {
        Grade.PhobiaScares++;
    }

    private void OnTimeLeftChanged(float value)
    {
        Grade.TimeLeft = value;
    }
}
