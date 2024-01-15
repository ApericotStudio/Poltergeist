using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _optionsClosedButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Color _selectedColor;
    private ColorBlock color;
    private List<Button> _buttons = new List<Button>();

    [Header("Canvas References")]
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    [Header("Scene References")]
    [SerializeField] private string _introCutsceneScene;
    [SerializeField] private string _creditsSceneName;

    [Header("Links")]
    [SerializeField] private string _feedbackFormLink = string.Empty;
    [SerializeField] private string _wishlistLink = string.Empty;

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
        _creditsButton.onClick.AddListener(OnCreditsButtonPressed);

        _buttons.Add(_playButton);
        _buttons.Add(_optionsButton);
        _buttons.Add(_exitButton);
        _buttons.Add(_feedbackButton);
        _buttons.Add(_creditsButton);

        foreach (Button button in _buttons)
        {
            color = button.colors;
            color.selectedColor = _selectedColor;

            button.colors = color;
        }
    }

    private void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(_introCutsceneScene);
    }

    private void OnOptionsButtonPressed()
    {
        _settingsCanvas.SetActive(true);
    }

    private void OnExitButtonPressed()
    {
        Application.Quit();
    }

    private void OnFeedbackButtonPressed()
    {
        Application.OpenURL(_feedbackFormLink);
    }

    private void OnCreditsButtonPressed()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }
}
