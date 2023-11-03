using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
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
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private TMP_Text _gameOverText;

    public UnityEvent<GameEvents> OnGameEvent { get => _onGameEvent; set => _onGameEvent = value; }

    private void Awake()
    {
        OnGameEvent.Invoke(GameEvents.Playing);
        OnGameEvent.AddListener(OnGameEventOccurred);
    }

    private void OnGameEventOccurred(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerLost)
        {
            _gameOverText.text = "You lost!";
            _menu.SetActive(true);
            Time.timeScale = 0;
        }
        if (gameEvent == GameEvents.PlayerWon)
        {
            _gameOverText.text = "You won!";
            _menu.SetActive(true);
            Time.timeScale = 0;
        }
        if (gameEvent == GameEvents.Playing)
        {
            _menu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
