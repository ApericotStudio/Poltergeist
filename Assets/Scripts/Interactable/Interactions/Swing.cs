using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField]
    private Vector3 _swingDirection;
    public void Activate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(_swingDirection, ForceMode.Impulse);
    }
}
