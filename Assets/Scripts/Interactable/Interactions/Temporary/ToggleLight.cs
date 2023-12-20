using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    private ParticleSystem _particleSystem;
    public void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    public void Toggle()
    {
        _light.enabled = !_light.enabled;

        if (_light.enabled)
        {
            _particleSystem.gameObject.SetActive(true);
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.gameObject.SetActive(false);
            _particleSystem.Stop();
        }
    }
}
