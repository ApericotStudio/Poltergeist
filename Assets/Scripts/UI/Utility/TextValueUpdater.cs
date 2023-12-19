using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextValueUpdater : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GetComponentInParent<Slider>().onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _text.text = value.ToString();
    }
}
