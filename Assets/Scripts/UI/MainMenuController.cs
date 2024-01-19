using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private Button _creditsButton;

    [Header("Canvas References")]
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private Animator _canvasAnimator;

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

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _canvasAnimator?.SetBool("Pressed", true);
        }
    }

    private void SetupButtons()
    {
        _playButton.onClick.AddListener(OnPlayButtonPressed);
        _optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPressed);
        _feedbackButton.onClick.AddListener(OnFeedbackButtonPressed);
        _creditsButton.onClick.AddListener(OnCreditsButtonPressed);
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
