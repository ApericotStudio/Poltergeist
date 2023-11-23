using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : MonoBehaviour
{
    public GameObject book;

    [SerializeField] private int nrOfBooks;
    private int bookCounter = 0;
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
        if (bookCounter < nrOfBooks)
        {
            GameObject myBook = Instantiate(book, spawnLocation.transform.position, Quaternion.identity);
            Rigidbody rb = myBook.GetComponent<Rigidbody>();
            rb.AddForce(direction * throwForce);
            bookCounter++;
        }
    }
}
