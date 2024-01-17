using StarterAssets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum PlayerPrefsVariable
{
    Volume,
    Sensitivity,
    Brightness,
    Contrast,
    Fullscreen,
    Tutorial
}

public class OptionsController : MonoBehaviour
{
    [Header("Slider References")]
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Slider _contrastSlider;

    [Header("Toggle References")]
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Toggle _tutorialToggle;

    [Header("Button References")]
    [SerializeField] private Button _backButton;

    [Header("Other References")]
    [Tooltip("Canvas that is opened when back button is pressed")]
    [SerializeField] private GameObject _backCanvas;
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Player References")]
    [Tooltip("The player in the game")]
    [SerializeField] private ThirdPersonController _thirdPersonController;

    private void Awake()
    {
        SetupSliders();
        SetupToggles();
        SetupButtons();
    }

    private void OnEnable()
    {
        SetSliders();
        SetToggles();
    }

    private void SetupSliders()
    {
        _volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        _sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        _brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderValueChanged);
        _contrastSlider.onValueChanged.AddListener(OnContrastSliderValueChanged);
    }

    private void SetSliders()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _sensitivitySlider.value = PlayerPrefs.GetFloat(PlayerPrefsVariable.Sensitivity.ToString(), 1);
        _brightnessSlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Brightness.ToString(), 100);
        _contrastSlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Contrast.ToString(), 100);
    }

    private void SetupToggles()
    {
        _fullscreenToggle.onValueChanged.AddListener(OnFullscreenTogglePressed);
        _tutorialToggle.onValueChanged.AddListener(OnTutorialTogglePressed);
    }

    private void SetToggles()
    {
        _fullscreenToggle.isOn = PlayerPrefs.GetInt(PlayerPrefsVariable.Fullscreen.ToString(), Screen.fullScreen ? 1 : 0) == 1;
        _tutorialToggle.isOn = PlayerPrefs.GetInt(PlayerPrefsVariable.Tutorial.ToString()) == 0;
    }

    private void SetupButtons()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnVolumeSliderValueChanged(float value)
    {
        _audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(PlayerPrefsVariable.Volume.ToString(), value);
        PlayerPrefs.Save();
    }

    private void OnSensitivitySliderValueChanged(float value)
    {
        _thirdPersonController?.SetSensitivity(value);
        PlayerPrefs.SetFloat(PlayerPrefsVariable.Sensitivity.ToString(), value);
        PlayerPrefs.Save();
    }

    private void OnBrightnessSliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Brightness.ToString(), (int)value);
        PlayerPrefs.Save();
    }

    private void OnContrastSliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Contrast.ToString(), (int)value);
        PlayerPrefs.Save();
    }

    private void OnFullscreenTogglePressed(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt(PlayerPrefsVariable.Fullscreen.ToString(), value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnTutorialTogglePressed(bool value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Tutorial.ToString(), value ? 0 : 1);
        PlayerPrefs.Save();
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
        _backCanvas.SetActive(true);
    }
}
