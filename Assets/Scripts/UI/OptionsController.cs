using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        SetupSliders();
        SetupToggles();
        SetupButtons();
    }

    private void Start()
    {
        Screen.fullScreen = PlayerPrefs.GetInt(PlayerPrefsVariable.Fullscreen.ToString(), Screen.fullScreen ? 1 : 0) == 1;
        Screen.brightness = PlayerPrefs.GetInt(PlayerPrefsVariable.Brightness.ToString(), 100);
        float volume = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _audioMixer.SetFloat("GameVol", volume);
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
        _audioMixer.SetFloat("GameVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(PlayerPrefsVariable.Volume.ToString(), value);
        PlayerPrefs.Save();
    }

    private void OnSensitivitySliderValueChanged(float value)
    {
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
        PlayerPrefs.SetInt(PlayerPrefsVariable.Fullscreen.ToString(), value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
        _backCanvas.SetActive(true);
    }
}
