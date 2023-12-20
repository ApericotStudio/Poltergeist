using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private Button _wishlistButton;

    [Header("Canvas References")]
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;

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
        _wishlistButton.onClick.AddListener(OnWishlistButtonPressed);
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

    private void OnWishlistButtonPressed()
    {
        Application.OpenURL(_wishlistLink);
    }
}
