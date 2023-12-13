using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Button _backButton;
    [Tooltip("Canvas that will be activated upon pressing the back button")]
    [SerializeField] private GameObject _backDestinationCanvas;
    [SerializeField] private SettingsData _settingsData;

    private void Awake()
    {
        SetupButtonsAndSliders();
    }

    private void SetupButtonsAndSliders()
    {
        _sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnSensitivitySliderValueChanged(float value)
    {
        _settingsData.Sensitivity = value;
    }

    private void OnSoundVolumeSliderValueChanged(float value)
    {
        _settingsData.SoundVolume = value;
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        _settingsData.MusicVolume = value;
    }

    private void OnBackButtonPressed()
    {
        _backDestinationCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
