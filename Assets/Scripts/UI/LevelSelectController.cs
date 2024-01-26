using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _assignmentButton;
    [SerializeField] private Button _finalExamButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Color _selectedColor;
    private ColorBlock color;
    private List<Button> _buttons = new();

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI _levelTitleText;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;

    [Header("Image References")]
    [SerializeField] private Image _gradeImage;
    [SerializeField] private Image _postItNote;
    [SerializeField] private List<Image> _levelSelectedImages = new();

    [Header("Scene References")]
    [SerializeField] private string _mainMenuScene;

    [Header("Other")]
    [SerializeField] private LevelCatalog _levelCatalog;
    [SerializeField] private GradeConverter _gradeConverter;

    private Level _selectedLevel;
    private Sprite _gradeImagePlaceholder;

    private void Awake()
    {
        SetupButtons();
        _gradeImagePlaceholder = _gradeImage.sprite;
        RetrieveProgress();
    }

    private void SetupButtons()
    {
        _assignmentButton.onClick.AddListener(OnAssignmentButtonPressed);
        _finalExamButton.onClick.AddListener(OnFinalExamButtonPressed);
        _startButton.onClick.AddListener(OnStartButtonPressed);
        _backButton.onClick.AddListener(OnBackButtonPressed);

        _buttons.Add(_assignmentButton);
        _buttons.Add(_finalExamButton);
        _buttons.Add(_startButton);
        _buttons.Add(_backButton);

        foreach (Button button in _buttons)
        {
            color = button.colors;
            color.selectedColor = _selectedColor;

            button.colors = color;
        }
    }

    private void RetrieveProgress()
    {
        LevelGradeHandler levelGradeHandler = new LevelGradeHandler();
        Grade grade = levelGradeHandler.Load(_levelCatalog.Levels[0].SceneName);
        if (grade == null)
        {
            SelectLevel(0);
            _postItNote.enabled = true;
            _finalExamButton.interactable = false;
        }
        else
        {
            SelectLevel(1);
            _postItNote.enabled = false;
            _finalExamButton.interactable = true;
        }
    }

    private void OnAssignmentButtonPressed()
    {
        SelectLevel(0);
    }

    private void OnFinalExamButtonPressed()
    {
        SelectLevel(1);
    }

    private void OnStartButtonPressed()
    {
        SceneManager.LoadScene(_selectedLevel.SceneName);        
    }

    private void OnBackButtonPressed()
    {
        SceneManager.LoadScene(_mainMenuScene);
    }

    private void Update()
    {

    }
    private void SelectLevel(int levelIndex)
    {
        if (!LevelUnlocked(levelIndex))
        {
            return;
        }
        foreach (Image levelSelectedImage in _levelSelectedImages)
        {
            levelSelectedImage.enabled = false;
        }
        _levelSelectedImages[levelIndex].enabled = true;
        Level level = _levelCatalog.Levels[levelIndex];
        LevelGradeHandler levelGradeHandler = new LevelGradeHandler();
        Grade grade = levelGradeHandler.Load(level.SceneName);
        if (grade != null)
        {
            _gradeImage.sprite = _gradeConverter.GetGradeSprite(grade.Result);
        }
        else
        {
            _gradeImage.sprite = _gradeImagePlaceholder;
        }
        _levelTitleText.text = level.Title;
        _levelDescriptionText.text = level.Description;
        _selectedLevel = level;
    }

    /// <summary>
    /// Checks if previous level has a grade
    /// </summary>
    private bool LevelUnlocked(int levelIndex)
    {
        if (levelIndex == 0)
        {
            return true;
        }
        Level previousLevel = _levelCatalog.Levels[levelIndex - 1];
        LevelGradeHandler levelGradeHandler = new LevelGradeHandler();
        Grade grade = levelGradeHandler.Load(previousLevel.SceneName);
        bool hasGrade = grade != null;
        return hasGrade;
    }
}
