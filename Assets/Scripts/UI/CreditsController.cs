using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private GameObject _audioCredits;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
        _nextButton.onClick.AddListener(OnNextButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    private void OnNextButtonPressed()
    {
        _audioCredits.SetActive(true);
    }
}
