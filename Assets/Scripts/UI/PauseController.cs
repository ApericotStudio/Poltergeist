using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Button References")]
    public Button ResumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _buttonMapButton;
    [SerializeField] private Button _mainMenuButton;

    [Header("UI References")]
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _buttonMap;
    [SerializeField] private GameObject _quitConfirmation;
    private Canvas _pauseCanvas;

    [Header("Progress References")]
    [SerializeField] private TextMeshProUGUI _timePassed;
    [SerializeField] private TextMeshProUGUI _phobiaScares;
    [SerializeField] private TextMeshProUGUI _differentObjectsUsed;

    [Header("Other References")]
    [SerializeField] private GradeController _gradeController;

    [SerializeField] private Transform _visitorOverlayParent;
    [SerializeField] private GameObject _visitorCollection;

    private void Awake()
    {
        SetupButtons();
        AddNpcOverlays();
        _pauseCanvas = GetComponent<Canvas>();
    }

    public void TogglePause(bool enable)
    {
        if (enable)
        {
            _pauseCanvas.enabled = true;
            SetProgress(_gradeController.Grade);
        }
        else
        {
            _buttonMap.SetActive(false);
            _quitConfirmation.SetActive(false);
            _options.SetActive(false);
            _pauseCanvas.enabled = false;
        }
    }

    private void SetupButtons()
    {
        ResumeButton.onClick.AddListener(OnResumeButtonPressed);
        _optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        _buttonMapButton.onClick.AddListener(OnButtonMapButtonPressed);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }

    private void OnResumeButtonPressed()
    {
        TogglePause(false);
    }

    private void OnOptionsButtonPressed()
    {
        _options.SetActive(true);
    }

    private void OnButtonMapButtonPressed()
    {
        _buttonMap.SetActive(true);
    }

    private void OnMainMenuButtonPressed()
    {
        _quitConfirmation.SetActive(true);
    }

    private void SetProgress(Grade grade)
    {
        _timePassed.text = grade.TimePassed.ToString();
        _phobiaScares.text = grade.PhobiaScares.ToString();
        _differentObjectsUsed.text = grade.DifferentObjectsUsed.ToString();
    }

    private void AddNpcOverlays()
    {
        foreach (VisitorController controller in _visitorCollection.GetComponentsInChildren<VisitorController>())
        {
            GameObject visitorOverlay = Instantiate(controller.VisitorsPausePrefab, _visitorOverlayParent);
            visitorOverlay.GetComponent<VisitorOverlayController>().Setup(controller);
        }
    }
}
