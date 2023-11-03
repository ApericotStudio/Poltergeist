using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum GameEvents 
{
    Playing,
    GameOver
}
public class GameEventManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<GameEvents> _onGameEvent;
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private TMP_Text _gameOverText;

    private bool _won;
    public bool Won { get => _won; set => _won = value; }
    public UnityEvent<GameEvents> OnGameEvent { get => _onGameEvent; set => _onGameEvent = value; }

    private void Awake()
    {
        OnGameEvent.Invoke(GameEvents.Playing);
        OnGameEvent.AddListener(OnGameEventOccurred);
    }

    private void OnGameEventOccurred(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.GameOver)
        {
            _gameOverText.text = _won ? "You Won!" : "You Lost!";
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
