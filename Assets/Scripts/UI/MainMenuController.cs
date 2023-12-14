using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private GameObject _levelSelectCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    private string _feedbackFormLink = "https://www.google.com/search?sca_esv=590933568&rlz=1C1VDKB_nlNL1073NL1073&sxsrf=AM9HkKkK5Zm03OJ3kJ9JTsjPTryE5VUblw:1702571829163&q=monke&tbm=isch&source=lnms&sa=X&ved=2ahUKEwj4q9Ooro-DAxVKhv0HHRtRADgQ0pQJegQIDxAB&biw=1280&bih=559&dpr=1.5#imgrc=U4fq2R8KOKmf_M";

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        _playButton.onClick.AddListener(OnPlayButtonPressed);
        _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPressed);
        _feedbackButton.onClick.AddListener(OnFeedbackButtonPressed);
    }

    private void OnPlayButtonPressed()
    {
        _levelSelectCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSettingsButtonPressed()
    {
        _settingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnExitButtonPressed()
    {
        Debug.Log("Exit Game");
    }

    private void OnFeedbackButtonPressed()
    {
        Application.OpenURL(_feedbackFormLink);
    }
}
