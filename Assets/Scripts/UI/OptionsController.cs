using UnityEngine;
using UnityEngine.UI;

public enum PlayerPrefsVariable
{
    Volume,
    Sensitivity,
    Brightness,
    Contrast,
    Fullscreen
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

    [Header("Button References")]
    [SerializeField] private Button _backButton;

    [Header("Other References")]
    [Tooltip("Canvas that is opened when back button is pressed")]
    [SerializeField] private GameObject _backCanvas;

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
        _volumeSlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Volume.ToString(), 100);
        _sensitivitySlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Sensitivity.ToString(), 100);
        _brightnessSlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Brightness.ToString(), 100);
        _contrastSlider.value = PlayerPrefs.GetInt(PlayerPrefsVariable.Contrast.ToString(), 100);
    }

    private void SetupToggles()
    {
        _fullscreenToggle.onValueChanged.AddListener(OnFullscreenTogglePressed);
    }

    private void SetToggles()
    {
        _fullscreenToggle.isOn = PlayerPrefs.GetInt(PlayerPrefsVariable.Fullscreen.ToString(), Screen.fullScreen ? 1 : 0) == 1;
    }

    private void SetupButtons()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnVolumeSliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Volume.ToString(), (int)value);
    }

    private void OnSensitivitySliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Sensitivity.ToString(), (int)value);
    }

    private void OnBrightnessSliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Brightness.ToString(), (int)value);
    }

    private void OnContrastSliderValueChanged(float value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Contrast.ToString(), (int)value);
    }

    private void OnFullscreenTogglePressed(bool value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariable.Fullscreen.ToString(), value ? 1 : 0);
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
        _backCanvas.SetActive(true);
    }
}
