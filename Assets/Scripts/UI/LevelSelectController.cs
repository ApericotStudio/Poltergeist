using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelCatalog _levelCatalog;
    [SerializeField] private GameObject _levelCardPrefab;
    [SerializeField] private GameObject _levelCardContainer;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GradeDisplay _gradeDisplay;
    [SerializeField] private TextMeshProUGUI _levelDescription;

    private void Awake()
    {
        SetupLevelCards();
        SetupButtons();
    }

    private void SetupLevelCards()
    {
        foreach(Level level in _levelCatalog.Levels)
        {
            GameObject levelCard = Instantiate(_levelCardPrefab, _levelCardContainer.transform);
            LevelCardController levelCardController = levelCard.GetComponent<LevelCardController>();
            levelCardController.Setup(level);
            levelCardController.OnLevelSelected += OnLevelSelected;
        }
    }

    private void SetupButtons()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        _mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnLevelSelected(Level level)
    {
        _gradeDisplay.SetGrade(level.Grade);
        _levelDescription.text = level.Description;
    }
}
