using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _resitButton;
    [SerializeField] private Button _levelSelectButton;
    [SerializeField] private Button _mainMenuButton;

    [Header("Text references")]
    [SerializeField] private TextMeshProUGUI _timePassed;
    [SerializeField] private TextMeshProUGUI _phobiaScares;
    [SerializeField] private TextMeshProUGUI _differentObjectsUsed;

    [Header("Other")]
    [SerializeField] private GradeController _gradeController;
    [SerializeField] private string _mainMenuSceneName = "MainMenuUI";
    [SerializeField] private string _levelSelectSceneName = "LevelSelectUI";
    [SerializeField] private string _endCutsceneSceneName = "EndCutsceneUI";
    [SerializeField] private Image _gradeImage;
    [SerializeField] private Image _gradeImageSpeed;
    [SerializeField] private Image _gradeImageResearch;
    [SerializeField] private Image _gradeImageResource;
    [SerializeField] private GradeConverter _gradeConverter;

    private Grade result;
    private void Awake()
    {
        _resitButton.onClick.AddListener(OnResitButtonClicked);
        _levelSelectButton.onClick.AddListener(OnLevelSelectButtonPressed);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);

        result = _gradeController.Grade;
    }

    public void OnEnable()
    {
        Time.timeScale = 0f;
        UpdateResults();
        CheckForHighestGrade();
    }

    /// <summary>
    /// Update values on result screen
    /// </summary>
    private void UpdateResults()
    {
        _timePassed.text = result.TimePassed.ToString() + " seconds";
        if(_phobiaScares != null)
        {
            _phobiaScares.text = result.PhobiaScares.ToString() + " times";
        }
        _differentObjectsUsed.text = result.DifferentObjectsUsed.ToString() + " objects used";

        _gradeImage.sprite = _gradeConverter.GetGradeSprite(result.Result);
        _gradeImageSpeed.sprite = _gradeConverter.GetGradeSprite(result.TimePassedScore);
        if (_gradeImageResearch)
        {
            _gradeImageResearch.sprite = _gradeConverter.GetGradeSprite(result.PhobiaScaresScore);
        }
        _gradeImageResource.sprite = _gradeConverter.GetGradeSprite(result.DifferentObjectsUsedScore);
    }

    /// <summary>
    /// Checks if player has achieved a new highest grade and if so save it
    /// </summary>
    private void CheckForHighestGrade()
    {
        LevelGradeHandler levelGradeHandler = new LevelGradeHandler();
        Grade currentGrade = levelGradeHandler.Load(SceneManager.GetActiveScene().name);

        bool newHighestGrade = false;
        if (currentGrade == null)
        {
            newHighestGrade = true;
        }
        else if (currentGrade.Result < _gradeController.Grade.Result)
        {
            newHighestGrade = true;
        }
        if (newHighestGrade)
        {
            levelGradeHandler.Save(_gradeController.Grade, SceneManager.GetActiveScene().name);
        }
    }

    private void OnResitButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnLevelSelectButtonPressed()
    {
        Time.timeScale = 1.0f;
        if (SceneManager.GetActiveScene().name == "FinalExam")
        {
            SceneManager.LoadScene(_endCutsceneSceneName);
        }
        else
        {
            SceneManager.LoadScene(_levelSelectSceneName);
        }
    }

    private void OnMainMenuButtonPressed()
    {
        Time.timeScale = 1.0f;
        if (SceneManager.GetActiveScene().name == "FinalExam")
        {
            SceneManager.LoadScene(_endCutsceneSceneName);
        }
        else
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }
}
