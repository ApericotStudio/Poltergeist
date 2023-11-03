using UnityEngine;
using UnityEngine.Events;

public enum GameEvents 
{
    Playing,
    PlayerLost,
    PlayerWon
}
public class GameEventManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<GameEvents> _onGameEvent;
    private UIManager _uiManager;

    public UnityEvent<GameEvents> OnGameEvent { get => _onGameEvent; set => _onGameEvent = value; }

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
        OnGameEvent.Invoke(GameEvents.Playing);
        OnGameEvent.AddListener(OnGameEventOccurred);
    }

    private void OnGameEventOccurred(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerLost)
        {
            _uiManager.OpenLoseScreen();
        }
        if (gameEvent == GameEvents.PlayerWon)
        {
            _uiManager.OpenWinScreen();
        }
        if (gameEvent == GameEvents.Playing)
        {
            _uiManager.CloseMenu();
        }
    }
}
