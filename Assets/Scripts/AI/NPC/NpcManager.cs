using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public delegate void NpcsLeftChanged(int amount);
    public event NpcsLeftChanged OnNpcsLeftChanged;

    private NpcController[] _npcs;

    [Header("References")]
    [SerializeField] private GameObject _npcCollection;
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        SetupVisitors();
    }

    private void SetupVisitors()
    {
        _npcs = _npcCollection.GetComponentsInChildren<NpcController>();
        foreach(NpcController npcController in _npcs)
        {
            npcController.OnStateChange.AddListener(OnNpcStateChanged);
        }
        _gameData.AmountOfVisitors = _npcs.Length;
    }

    private void OnNpcStateChanged()
    {
        int scaredNpcCounter = 0;
        foreach(NpcController npcController in _npcs)
        {
            if (npcController.CurrentState is PanickedState)
            {
                scaredNpcCounter++;
            }
        }
        _gameData.AmountOfVisitorsScared = scaredNpcCounter;
        if (scaredNpcCounter == _npcs.Length)
        {
            _gameManager.EndGame();
        }
    }
}
