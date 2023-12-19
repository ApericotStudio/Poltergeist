using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [Header("Slider References")]
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Slider _contrastSlider;

    [Header("Toggle References")]
    [SerializeField] private Toggle _fullscreenToggle;

    [Header("Other References")]
    [SerializeField] private OptionsData _optionsData;

    private void Awake()
    {
        SetupSliders();
        SetupToggles();
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
        _volumeSlider.value = _optionsData.Volume;
        _sensitivitySlider.value = _optionsData.Sensetivity;
        _brightnessSlider.value = _optionsData.Brightness;
        _contrastSlider.value = _optionsData.Contrast;
    }

    private void SetupToggles()
    {
        _fullscreenToggle.onValueChanged.AddListener(OnFullscreenTogglePressed);
    }

    private void SetToggles()
    {
        _fullscreenToggle.isOn = _optionsData.Fullscreen;
    }

    private void OnVolumeSliderValueChanged(float value)
    {
        _optionsData.Volume = value;
    }

    private void OnSensitivitySliderValueChanged(float value)
    {
        _optionsData.Sensetivity = value;
    }

    private void OnBrightnessSliderValueChanged(float value)
    {
        _optionsData.Brightness = value;
    }

    private void OnContrastSliderValueChanged(float value)
    {
        _optionsData.Contrast = value;
    }

    private void OnFullscreenTogglePressed(bool value)
    {
        _optionsData.Fullscreen = value;
    }
}