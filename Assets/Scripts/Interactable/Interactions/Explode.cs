using UnityEngine;

public class Explode : MonoBehaviour
{
    public void Activate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
    }
}
