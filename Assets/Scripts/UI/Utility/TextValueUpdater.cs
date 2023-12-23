using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System;

public class TextValueUpdater : MonoBehaviour
{
    [SerializeField] private PlayerPrefsVariable _playerPrefVariable;

    private TextMeshProUGUI _text;

    private int _rounded;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        if (_playerPrefVariable == PlayerPrefsVariable.Volume || _playerPrefVariable ==  PlayerPrefsVariable.Sensitivity)
        {
            OnSliderValueChanged(PlayerPrefs.GetFloat(_playerPrefVariable.ToString(), 1));
        }
        else
        {
            OnSliderValueChanged(PlayerPrefs.GetInt(_playerPrefVariable.ToString(), 100));
        }

        GetComponentInParent<Slider>().onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        switch (_playerPrefVariable)
        {
            case PlayerPrefsVariable.Volume:
            _rounded = (int)Mathf.Round(value *= 100);
            break;

            case PlayerPrefsVariable.Sensitivity:
            float OneDecimal = (float)Math.Round(value, 1);
            _text.text = OneDecimal.ToString();
            break;

            default:
            _rounded = (int)value;
            break;
        }

        if (_playerPrefVariable == PlayerPrefsVariable.Sensitivity)
        {
            return;
        }

        _text.text = _rounded.ToString();
    }
}
