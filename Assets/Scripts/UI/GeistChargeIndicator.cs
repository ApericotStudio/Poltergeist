using UnityEngine;
using UnityEngine.UI;

public class GeistChargeIndicator : MonoBehaviour
{
    [SerializeField] private Image _filler;

    private void Awake()
    {
        GetComponentInParent<ObservableObject>().OnGeistChargeChanged += OnGeistChargeValueChange;
    }

    private void OnGeistChargeValueChange(float value)
    {
        _filler.fillAmount = value;
    }
}
