using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController _playerController;
    private GameManager _gameManager;
    private VisitorManager _visitorManager;
    private PolterSenseController _polterSenseController;
    private PossessionController _possessionController;
    private InteractController _interactController;
    private FearHandler[] _visitors;

    [SerializeField]
    private InGameUIController _uiController;
    [SerializeField]
    private GameObject _continueButton;
    private int _counter;
    private bool _firstTutorialShown = false;
    private bool _skippingTutorial = false;
    private bool _neverShow = false;
    [SerializeField]
    private GameObject _skipTutorialCanvas;
    [SerializeField]
    private GameObject _TutorialCanvas;

    public bool NeverShow { get => _neverShow; set => _neverShow = value; }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Assignment")
        {
            _firstTutorialShown = true;
        }
    }

    public void DisableButton(bool isOn)
    {
        _continueButton.SetActive(!isOn);
    }
    // Start is called before the first frame update
    void Start()
    {
        _polterSenseController = _playerController.GetComponent<PolterSenseController>();
        _possessionController = _playerController.GetComponent<PossessionController>();
        _interactController = _playerController.GetComponent<InteractController>();
        _visitorManager = gameObject.GetComponent<VisitorManager>();
        _visitors = _visitorManager.VisitorCollection.GetComponentsInChildren<FearHandler>();
        _gameManager = gameObject.GetComponent<GameManager>();

        int tutorial = PlayerPrefs.GetInt(PlayerPrefsVariable.Tutorial.ToString());
        int firstTutorial = PlayerPrefs.GetInt("FirstTutorial");

        _skipTutorialCanvas.SetActive(false);

        if (tutorial == 1)
        {
            SkipTutorial();
            return;
        }

        //after toggling never show and continue only first tutorial will be skipped
        if(firstTutorial == 1 && !_firstTutorialShown)
        {
            SkipTutorial();
            return;
        }

        if(firstTutorial == 1 && _firstTutorialShown)
        {
            StartTutorial();
            PlayerPrefs.SetInt(PlayerPrefsVariable.Tutorial.ToString(), 1);
            PlayerPrefs.Save();
            return;
        }

        if (_firstTutorialShown)
        {
            StartTutorial();
            return;
        }

        else
        {
            Time.timeScale = 0;
            _skipTutorialCanvas.SetActive(true);

        }
        _gameManager.UpdateCursor();

    }
    public void StartTutorial()
    {
        if (_neverShow)
        {
            PlayerPrefs.SetInt("FirstTutorial", 1);
            PlayerPrefs.Save();
        }

        _skipTutorialCanvas.SetActive(false);
        Time.timeScale = 1;
        _gameManager.UpdateCursor();
        showTutorial(0);
        _TutorialCanvas.SetActive(true);
    }

    public void SkipTutorial()
    {
        if (_neverShow)
        {
            PlayerPrefs.SetInt(PlayerPrefsVariable.Tutorial.ToString(), 1);
            PlayerPrefs.Save();
        }

        Time.timeScale = 1;
        _gameManager.UpdateCursor();
        _skipTutorialCanvas.SetActive(false);
        _uiController.StopTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_firstTutorialShown)
        {
            checkFirstTutorial();
        }
        else
        {
            checkSecondTutorial();
        }
    }

    private void showTutorial(int index)
    {
        if(_counter == index + 1)
        {
            return;
        }

        //previous tutorial popups don't appear again
        if (index <= _counter - 2)
        {
            return;
        }

        _uiController.ShowTutorial(index);
        _counter = index + 1;

        unsubscribeEvents();
    }

    private void CheckForPhobiaAchievement(int index)
    {
        Steamworks.SteamUserStats.GetAchievement("PhobiaExploit", out bool achievementUnlocked);

        if(!achievementUnlocked)
        {
            Steamworks.SteamUserStats.SetAchievement("PhobiaExploit");
            Steamworks.SteamUserStats.StoreStats();
        }
    }

    private void unsubscribeEvents()
    {
        _polterSenseController.isEnabled -= showTutorial;
        _interactController.hasInteracted -= showTutorial;
        _playerController.hasMoved -= showTutorial;
        _possessionController.hasPossessed -= showTutorial;

        foreach (FearHandler visitor in _visitors)
        {
            visitor.activatedPhobia -= showTutorial;
        }
    }

    private void checkFirstTutorial()
    {
        switch (_counter)
        {
            case 1:
                _playerController.hasMoved += showTutorial;
                break;
            case 2:
                _polterSenseController.isEnabled += showTutorial;
                break;
            case 3:
                _interactController.hasInteracted += showTutorial;
                break;
            case 4:
                _possessionController.hasPossessed += showTutorial;
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
        }
    }

    private void checkSecondTutorial()
    {
        switch (_counter)
        {
            case 0:
                break;
            case 1:
                foreach (FearHandler visitor in _visitors)
                {
                    visitor.activatedPhobia += showTutorial;
                    visitor.activatedPhobia += CheckForPhobiaAchievement;
                }
                break;
        }
    }
}
