using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextValueUpdater : MonoBehaviour
{
    [SerializeField] private PlayerPrefsVariable _playerPrefVariable;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = PlayerPrefs.GetInt(_playerPrefVariable.ToString(), 100).ToString();
        GetComponentInParent<Slider>().onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _text.text = value.ToString();
    }
}
