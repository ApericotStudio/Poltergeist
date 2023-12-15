using UnityEngine;

public class VisitorManager : MonoBehaviour
{
    public delegate void VisitorsLeftChanged(int value);
    public event VisitorsLeftChanged OnVisitorsLeftChanged;

    private VisitorController[] _visitors;
    private int _scaredVisitors;

    [Header("References")]
    public GameObject VisitorCollection;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        SetupVisitors();
    }

    private void SetupVisitors()
    {
        _visitors = VisitorCollection.GetComponentsInChildren<VisitorController>();
        foreach(VisitorController npcController in _visitors)
        {
            npcController.OnStateChange.AddListener(OnNpcStateChanged);
        }
    }

    private void OnNpcStateChanged(IState state)
    {
        if (state is not PanickedState)
        {
            return;
        }
        _scaredVisitors++;
        int visitorsLeft = _visitors.Length - _scaredVisitors;
        OnVisitorsLeftChanged?.Invoke(visitorsLeft);
        if (visitorsLeft <= 0)
        {
            _gameManager.EndGame();
        }
    }
}
