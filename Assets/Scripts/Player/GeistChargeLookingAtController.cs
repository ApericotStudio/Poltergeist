using UnityEngine;

public class GeistChargeLookingAtController : MonoBehaviour
{
    private VisionController _visionController;
    private GeistChargeIndicator _previousGeistChargeIndicator;

    private void Awake()
    {
        _visionController = GetComponent<VisionController>();
        _visionController.LookingAtChanged.AddListener(OnLookingAtChanged);
    }

    private void OnLookingAtChanged()
    {
        if (_previousGeistChargeIndicator != null)
        {
            _previousGeistChargeIndicator.BeindLookedAt = false;
            _previousGeistChargeIndicator = null;
        }
        if (_visionController.LookingAt == null)
        {
            return;
        }
        GeistChargeIndicator geistChargeIndicator = _visionController.LookingAt.GetComponentInChildren<GeistChargeIndicator>();
        if (geistChargeIndicator != null)
        {
            geistChargeIndicator.BeindLookedAt = true;
        }
        _previousGeistChargeIndicator = geistChargeIndicator;
    }
}
