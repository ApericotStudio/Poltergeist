using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public GameObject spawnedObject;

    [SerializeField] private int nrOfSpawns;
    private int spawnCounter = 0;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float throwForce = 500;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = spawnLocation.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void throwBook()
    {
        if (spawnCounter < nrOfSpawns)
        {
            GameObject mySpawn = Instantiate(spawnedObject, spawnLocation.transform.position, Quaternion.identity);
            Rigidbody rb = mySpawn.GetComponent<Rigidbody>();
            rb.AddForce(direction * throwForce);
            spawnCounter++;
        }
    }
}
