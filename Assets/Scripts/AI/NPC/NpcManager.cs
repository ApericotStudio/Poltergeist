using UnityEngine;

public class NpcManager : MonoBehaviour
{
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
        Debug.Log("npc state changed");
        int scaredNpcCounter = 0;
        foreach(NpcController npcController in _npcs)
        {
            Debug.Log(npcController.CurrentState.GetType().Name);
            if (npcController.CurrentState is ScaredState)
            {
                Debug.Log("scared npc");
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
