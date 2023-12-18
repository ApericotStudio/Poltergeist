using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _buttonMapButton;
    [SerializeField] private Button _mainMenuButton;

    [Header("UI References")]
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _buttonMap;

    [Header("Other")]
    [SerializeField] private string _mainMenuSceneName;

    private void Awake()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        // update progress
    }

    private void SetupButtons()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonPressed);
        _optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        _buttonMapButton.onClick.AddListener(OnButtonMapButtonPressed);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }

    private void OnResumeButtonPressed()
    {
        // unpause game
    }

    private void OnOptionsButtonPressed()
    {
        gameObject.SetActive(false);
        _options.SetActive(true);
    }

    private void OnButtonMapButtonPressed()
    {
        gameObject.SetActive(false);
        _buttonMap.SetActive(true);
    }

    private void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
