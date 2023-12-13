using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        _playButton.onClick.AddListener(OnPlayButtonPressed);
        _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPressed);
    }

    private void OnPlayButtonPressed()
    {
        _levelSelectCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSettingsButtonPressed()
    {
        _settingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnExitButtonPressed()
    {
        Debug.Log("Exit Game");
    }
}
