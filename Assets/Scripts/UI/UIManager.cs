using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _gameEndScreen;

    private void Awake()
    {
        _gameManager.OnEndGame.AddListener(OnGameEnd);
    }

    private void OnGameEnd()
    {
        _gameEndScreen.SetActive(true);
    }
}
