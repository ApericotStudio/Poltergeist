using System.Collections;
using UnityEngine;

public class CabinetPlateThrower : MonoBehaviour
{
    [Header("Cabinet Plate Thrower Settings")]
    [Tooltip("The gameobject that contains all the plates that will be thrown."), SerializeField]
    private GameObject _plateCollection;
    [Tooltip("The maximum force that will be applied to the plates."), Range(0f, 20f), SerializeField]
    private float _maxThrowForce = 20f;
    [Tooltip("The minimum force that will be applied to the plates."), Range(0f, 20f), SerializeField]
    private float _minThrowForce = 15f;
    [Tooltip("The randomness of the direction of the force applied to the plates."), Range(0f, 1f),SerializeField]
    private float _directionRandomness = 0.1f;
    [Tooltip("The rate at which the plates will be thrown per second."), Range(0f, 10f), SerializeField]
    private float _throwRate = 5f;

    private ObjectWithDoor _door;
    private Rigidbody[] _plateRigidbodies;
    
    private void Awake()
    {
        _door = GetComponent<ObjectWithDoor>();
        _plateRigidbodies = _plateCollection.GetComponentsInChildren<Rigidbody>();
    }

    public void Throw()
    {
        _door.Use();
        StartCoroutine(ThrowPlates());
    }

    private IEnumerator ThrowPlates()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Rigidbody plate in _plateRigidbodies)
        {
            plate.isKinematic = false;
            plate.AddForce((transform.right + ForceDirectionOffset()) * RandomThrowForce(), ForceMode.Impulse);
            yield return new WaitForSeconds(1/_throwRate);
            EnableObservableObjectCollision(plate);
        }
    }
    /// <summary>
    /// Enables the collision between plates
    /// </summary>
    private void EnableObservableObjectCollision(Rigidbody plate)
    {
        plate.GetComponent<Collider>().excludeLayers &= ~(1 << gameObject.layer);
    }

    /// <summary>
    /// Returns a random vector3 for the force direction with the given randomness
    /// </summary>
    private Vector3 ForceDirectionOffset()
    {
        return new(
                Random.Range(-_directionRandomness, _directionRandomness), 
                Random.Range(-_directionRandomness, _directionRandomness), 
                Random.Range(-_directionRandomness, _directionRandomness));
    }

    /// <summary>
    /// Returns a random float between the min and max throw force
    /// </summary>
    private float RandomThrowForce()
    {
        return Random.Range(_minThrowForce, _maxThrowForce);
    }
}
