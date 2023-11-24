using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public GameObject SpawnedObject;
    [SerializeField] private GameObject _spawnLocation;
    [SerializeField] private float _throwForce = 500;
    private Vector3 _direction;
    // Start is called before the first frame update
    void Start()
    {
        _direction = _spawnLocation.transform.position - transform.position;
        _direction.y = 0;
        _direction.Normalize();
    }

    public void throwBook()
    {
        GameObject mySpawn = Instantiate(SpawnedObject, _spawnLocation.transform.position, Quaternion.identity);
        Rigidbody rb = mySpawn.GetComponent<Rigidbody>();
        rb.AddForce(_direction * _throwForce);
    }
}
