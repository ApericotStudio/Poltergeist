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
        _direction.Normalize();
        _spawnLocation.transform.forward = _direction;
    }

    public void ThrowObject()
    {
        GameObject mySpawn = Instantiate(SpawnedObject, _spawnLocation.transform.position, Quaternion.identity);
        Rigidbody rb = mySpawn.GetComponent<Rigidbody>();
        rb.AddForce(_direction * _throwForce);
    }

    public void SpewObjects()
    {
        //Need the coroutine to wait between spawns.
        StartCoroutine(SpawnMultiple());
    }

    private IEnumerator SpawnMultiple()
    {
        for (int i = 0; i < _nrPerActivation; i++)
        {
            GameObject mySpawn = Instantiate(SpawnedObject, _spawnLocation.transform.position, Quaternion.identity);
            mySpawn.transform.forward = _direction;
            Rigidbody rb = mySpawn.GetComponent<Rigidbody>();

            //Take a random location in a circle and set the circle at some distance forward from the spawn point. Throw the object to that point.
            Vector2 circle = Random.insideUnitCircle * _coneRadius;
            Vector3 target = _spawnLocation.transform.position + _spawnLocation.transform.forward * 5 + _spawnLocation.transform.rotation * new Vector3(circle.x, circle.y);
            rb.AddForce(target.normalized * _throwForce);

            //Wait for fixedupdate, otherwise all objects spawn at the exact same moment at the exact same location and add more force
            //upward / downward than you add to the rigidbody, causing them to spawn in a tower.
            yield return new WaitForFixedUpdate();
        }
    }
}
