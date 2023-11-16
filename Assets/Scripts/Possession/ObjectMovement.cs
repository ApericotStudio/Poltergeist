using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(StarterAssetsInputs))]
public class ObjectMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Rigidbody _rb;
    private Throwable _throwable;
    private StarterAssetsInputs _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _throwable = GetComponent<Throwable>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_throwable.isPossessed)
        {
            _rb.AddForce(new Vector3(_input.Move.x * _speed, 0f, _input.Move.y * _speed), ForceMode.Impulse);
        }
    }
}
