using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameEventManager _gameEventManager;
    [SerializeField]
    private Button _playAgainButton;

    private void Awake()
    {
        _playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
    }
    
    public void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
