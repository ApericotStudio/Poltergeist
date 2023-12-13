using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GradeController _gradeController;
    [SerializeField] private GradeDisplay _gradeDisplay;
    [SerializeField] private Button _replayButton;
    [SerializeField] private LevelCatalog _levelCatalog;

    private void Awake()
    {
        _replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    public void OnEnable()
    {
        UpdateSummary();
        CheckForHighestGrade();
    }

    private void UpdateSummary()
    {
        _gradeDisplay.SetGrade(_gradeController.Grade);
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

    private void OnReplayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
