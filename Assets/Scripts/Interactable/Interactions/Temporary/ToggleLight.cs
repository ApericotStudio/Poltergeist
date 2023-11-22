using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    [SerializeField] private Light _light;

    public void Toggle()
    {
        _light.enabled = !_light.enabled;
    }
}
