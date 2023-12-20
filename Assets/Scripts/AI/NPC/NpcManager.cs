using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public delegate void NpcsLeftChanged(int value);
    public event NpcsLeftChanged OnNpcsLeftChanged;

    private NpcController[] _npcs;
    private int _scaredNpcs;

    [Header("References")]
    public GameObject NpcCollection;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        SetupVisitors();
    }

    private void SetupVisitors()
    {
        _npcs = NpcCollection.GetComponentsInChildren<NpcController>();
        foreach(NpcController npcController in _npcs)
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
        _scaredNpcs++;
        int npcsLeft = _npcs.Length - _scaredNpcs;
        OnNpcsLeftChanged?.Invoke(npcsLeft);
        if (npcsLeft <= 0)
        {
            _gameManager.EndGame();
        }
    }
}