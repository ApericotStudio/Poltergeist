using UnityEditor;
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

    private void CheckForHighestGrade()
    {
        bool newHighestGrade = false;

        if (_levelCatalog.GetCurrent().Grade == null)
        {
            newHighestGrade = true;
        }
        else if (_levelCatalog.GetCurrent().Grade.Result < _gradeController.Grade.Result)
        {
            newHighestGrade = true;
        }

        if (newHighestGrade)
        {
            AssetDatabase.DeleteAsset("Assets/Scripts/Game/Grades/" + _levelCatalog.GetCurrent().SceneName + "Grade");
            AssetDatabase.Refresh();
            AssetDatabase.RenameAsset(_gradeController.GradeAssetPath, _levelCatalog.GetCurrent().SceneName + "Grade");
            AssetDatabase.Refresh();
            _levelCatalog.GetCurrent().Grade = _gradeController.Grade;
        }
    }

    private void OnReplayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
