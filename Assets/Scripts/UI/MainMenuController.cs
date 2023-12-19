using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _feedbackButton;

    [Header("Canvas References")]
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    [Header("Other")]
    [SerializeField] private string _feedbackFormLink = string.Empty;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        _playButton.onClick.AddListener(OnPlayButtonPressed);
        _optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPressed);
        _feedbackButton.onClick.AddListener(OnFeedbackButtonPressed);
    }

    private void OnPlayButtonPressed()
    {
        _levelSelectCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnOptionsButtonPressed()
    {
        _settingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnExitButtonPressed()
    {
        Application.Quit();
    }

    private void OnFeedbackButtonPressed()
    {
        Application.OpenURL(_feedbackFormLink);
    }
}
