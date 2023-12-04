using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private TextMeshProUGUI _grade;
    [SerializeField] private Button _replayButton;
    
    private void Awake()
    {
        _replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    public void OnEnable()
    {
        UpdateSummary();
    }

    private void UpdateSummary()
    {
        _grade.text = _gameData.Grade;
    }

    private void OnReplayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
