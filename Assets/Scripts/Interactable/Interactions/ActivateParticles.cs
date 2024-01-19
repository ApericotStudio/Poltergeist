using UnityEngine;

public class ActivateParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void Toggle()
    {
        if(_particleSystem.isEmitting)
        {
            _particleSystem.Stop();
        }
        else
        {
            _particleSystem.Play();
        }
    }
}
