using TMPro;
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

    [Header("Progress References")]
    [SerializeField] private TextMeshProUGUI _timePassed;
    [SerializeField] private TextMeshProUGUI _phobiaScares;
    [SerializeField] private TextMeshProUGUI _differentObjectsUsed;

    [Header("Other")]
    [SerializeField] private string _mainMenuSceneName;
    [SerializeField] private GradeController _gradeController;

    private void Awake()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        UpdateProgress(_gradeController.Grade);
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
        gameObject.SetActive(false);
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

    private void UpdateProgress(Grade grade)
    {
        _timePassed.text = grade.TimePassed.ToString();
        _phobiaScares.text = grade.PhobiaScares.ToString();
        _differentObjectsUsed.text = grade.DifferentObjectsUsed.ToString();
    }
}
