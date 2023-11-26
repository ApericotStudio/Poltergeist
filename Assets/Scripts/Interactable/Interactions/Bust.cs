using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bust : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Push()
    {
        _rigidbody.AddTorque(transform.forward, ForceMode.Impulse);
    }
}
