using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GradeController _gradeController;
    [SerializeField] private GradeDisplay _gradeDisplay;
    [SerializeField] private Button _replayButton;
    
    private void Awake()
    {
        _replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    public void OnEnable()
    {
        UpdateSummary();
    }

    private void UpdateSummary()
    {
        _gradeDisplay.SetGrade(_gradeController.Grade);
    }

    private void OnReplayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
