using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _assignmentButton;
    [SerializeField] private Button _finalExamButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI _levelTitleText;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;

    [Header("Canvas References")]
    [SerializeField] private GameObject _mainMenuCanvas;

    [Header("Scene References")]
    [SerializeField] private string _introCutsceneScene;

    [Header("Other")]
    [SerializeField] private LevelCatalog _levelCatalog;

    private Level _selectedLevel;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        _assignmentButton.onClick.AddListener(OnAssignmentButtonPressed);
        _finalExamButton.onClick.AddListener(OnFinalExamButtonPressed);
        _startButton.onClick.AddListener(OnStartButtonPressed);
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnAssignmentButtonPressed()
    {
        SelectLevel(_levelCatalog.Levels[0]);
    }

    private void OnFinalExamButtonPressed()
    {
        SelectLevel(_levelCatalog.Levels[1]);
    }

    private void OnStartButtonPressed()
    {
        int hasSeenCutscene = PlayerPrefs.GetInt("HasWatchedIntroCutscene", 0);
        if(hasSeenCutscene == 0)
        {
            SceneManager.LoadScene(_introCutsceneScene);
        }
        else
        {
            SceneManager.LoadScene(_selectedLevel.SceneName);
        }        
    }

    private void OnBackButtonPressed()
    {
        _mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SelectLevel(Level level)
    {
        LevelGradeHandler levelGradeHandler = new LevelGradeHandler();
        Grade grade = levelGradeHandler.Load(level.SceneName);
        // update grade and grade image
        _levelTitleText.text = level.Title;
        _levelDescriptionText.text = level.Description;
        _selectedLevel = level;
    }
}
