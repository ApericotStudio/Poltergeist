using System.Collections.Generic;
using UnityEngine;

public class GradeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _gameManagers;

    private Grade grade;
    private List<ObservableObject> _observableObjectsUsed = new List<ObservableObject>();

    private void Awake()
    {
        grade = new Grade();
        SetupSubscriptions();
    }

    private void SetupSubscriptions()
    {
        NpcManager npcManager = _gameManagers.GetComponent<NpcManager>();
        npcManager.OnNpcsLeftChanged += OnNpcsLeftChanged;
        foreach(FearHandler fearHandler in npcManager.NpcCollection.GetComponentsInChildren<FearHandler>())
        {
            fearHandler.OnObjectUsed += OnObjectUsed;
        }
        _gameManagers.GetComponent<GameManager>().OnTimeLeftChanged += OnTimeLeftChanged;
    }

    private void OnNpcsLeftChanged(int value)
    {
        grade.VisitorsLeft = value;
    }

    private void OnObjectUsed(ObservableObject observableObject)
    {
        if (_observableObjectsUsed.Contains(observableObject))
        {
            return;
        }
        _observableObjectsUsed.Add(observableObject);
        grade.DifferentObjectsUsed++;
    }

    private void OnPhobiaScare()
    {
        grade.PhobiaScares++;
    }

    private void OnTimeLeftChanged(float value)
    {
        grade.TimeLeft = value;
    }
}
