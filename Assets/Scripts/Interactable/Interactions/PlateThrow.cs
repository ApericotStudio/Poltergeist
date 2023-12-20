using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlateThrow : MonoBehaviour
{
    [SerializeField]
    private GameObject _plateCollection;
    private ObjectWithDoor _door;
    private Rigidbody[] _plateRigidbodies;
    private void Awake()
    {
        _door = GetComponent<ObjectWithDoor>();
        _plateRigidbodies = _plateCollection.GetComponentsInChildren<Rigidbody>();
        TogglePlatesPhysics();
    }

    public void Throw()
    {
        _door.Use();
        StartCoroutine(ThrowPlates());
    }

    private IEnumerator ThrowPlates()
    {
        TogglePlatesPhysics();
        yield return new WaitForSeconds(0.5f);
        foreach (Rigidbody plate in _plateRigidbodies)
        {
            plate.velocity = transform.right * Random.Range(15f, 20f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void TogglePlatesPhysics()
    {
        foreach (Rigidbody plate in _plateRigidbodies)
        {
            plate.isKinematic = !plate.isKinematic;
        }
    }
}
