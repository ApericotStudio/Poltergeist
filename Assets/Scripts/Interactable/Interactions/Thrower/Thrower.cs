using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Thrower : MonoBehaviour
{
    [Header("Object Thrower Settings")]
    [Tooltip("The gameobject that will be used as the direction of the force applied to the objects."), SerializeField]
    private GameObject _thrower;
    [Tooltip("The gameobject that contains all the objects that will be thrown."), SerializeField]
    private GameObject _objectCollection;
    [Tooltip("The maximum force that will be applied to the objects."), Range(0f, 20f), SerializeField]
    private float _maxThrowForce = 20f;
    [Tooltip("The minimum force that will be applied to the objects."), Range(0f, 20f), SerializeField]
    private float _minThrowForce = 15f;
    [Tooltip("The randomness of the direction of the force applied to the objects."), Range(0f, 1f),SerializeField]
    private float _directionRandomness = 0.1f;
    [Tooltip("The rate at which the objects will be thrown per second."), Range(0f, 10f), SerializeField]
    private float _throwRate = 5f;

    protected List<Rigidbody> _objectRigidbodies;

    private readonly System.Random rand = new();

    protected virtual void Awake()
    {
        _objectRigidbodies = _objectCollection.GetComponentsInChildren<Rigidbody>().ToList();
    }

    public abstract void Throw();

    protected IEnumerator ThrowObjects(int amount = 0, bool random = false, int amountThrownAtATime = 0)
    {
        if (amount == 0)
        {
            amount = _objectRigidbodies.Count;
        }

        if (amountThrownAtATime == 0)
        {
            amountThrownAtATime = amount;
        }

        Rigidbody randomObject = null;
        
        for (int i = 0; i < amount; i += amountThrownAtATime)
        {
            int currentAmount = System.Math.Min(amountThrownAtATime, amount - i);
            for (int j = i; j < i + currentAmount; j++)
            {
                if (_objectRigidbodies.Count == 0)
                {
                    break;
                }
                
                int randomIndex = rand.Next(_objectRigidbodies.Count);
                randomObject = _objectRigidbodies[randomIndex];
                _objectRigidbodies.RemoveAt(randomIndex);

                ThrowObject(randomObject, random);
            }

            yield return new WaitForSeconds(1 / _throwRate);
            if(randomObject != null)
            {
                EnableObservableObjectCollision(randomObject);
            }
        }
    }

    private void ThrowObject(Rigidbody objectRigidbody, bool random = false)
    {
        objectRigidbody.isKinematic = false;
        Vector3 forceDirection = _thrower.transform.forward + ForceDirectionOffset();
        float force = random ? RandomThrowForce() : _maxThrowForce;

        objectRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
    }
    
    /// <summary>
    /// Enables the collision between objects
    /// </summary>
    private void EnableObservableObjectCollision(Rigidbody objectRigidbody)
    {
        objectRigidbody.GetComponent<Collider>().excludeLayers &= ~(1 << gameObject.layer);
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
