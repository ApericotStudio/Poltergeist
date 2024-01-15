using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    }

    private void OnEnable()
    {
        SetProgress(_gradeController.Grade);
        EventSystem.current.SetSelectedGameObject(_optionsButton.gameObject);
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
        gameObject.SetActive(false);
    }

    private void OnOptionsButtonPressed()
    {
        gameObject.SetActive(false);
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
