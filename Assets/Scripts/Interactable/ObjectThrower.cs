using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public GameObject SpawnedObject;
    [SerializeField] private GameObject _spawnLocation;
    [SerializeField] private float _throwForce = 500;
    private Vector3 _direction;

    [Header("Spew Mode")]
    [SerializeField] private int _nrPerActivation = 20;
    [SerializeField] private float _coneRadius = 50;
    // Start is called before the first frame update
    void Start()
    {
        _direction = _spawnLocation.transform.position - transform.position;
        _direction.y = 0;
        _direction.Normalize();
        _spawnLocation.transform.forward = _direction;
    }

    public void throwBook()
    {
        GameObject mySpawn = Instantiate(SpawnedObject, _spawnLocation.transform.position, Quaternion.identity);
        Rigidbody rb = mySpawn.GetComponent<Rigidbody>();
        rb.AddForce(_direction * _throwForce);
    }

    public void SpewObjects()
    {
        StartCoroutine(spawnMultiple());
    }

    public IEnumerator spawnMultiple()
    {
        for (int i = 0; i<_nrPerActivation; i++)
        {
            GameObject mySpawn = Instantiate(SpawnedObject, _spawnLocation.transform.position, Quaternion.identity);
            mySpawn.transform.forward = _direction;
            Rigidbody rb = mySpawn.GetComponent<Rigidbody>();
            Vector2 circle = Random.insideUnitCircle * _coneRadius;
            Vector3 target = _spawnLocation.transform.position + _spawnLocation.transform.forward * 5 + _spawnLocation.transform.rotation * new Vector3(circle.x, circle.y);
            rb.AddForce(target.normalized* _throwForce);
            yield return new WaitForFixedUpdate();
        }
    }
}