using System.Collections;
using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private int _emitAmount = 10;
    [SerializeField]
    private float _emitRate = 3;

    public void Emit()
    {
        StartCoroutine(EmitOverTime());
    }

    private IEnumerator EmitOverTime()
    {
        for (int i = 0; i < _emitAmount; i++)
        {
            _particleSystem.Emit(1);
            yield return new WaitForSeconds(1f / _emitRate); // Emit 3 particles per second
        }
    }
}