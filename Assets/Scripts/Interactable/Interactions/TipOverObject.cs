using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TipOverObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _torque = new Vector3(1, 0, 0);


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Push()
    {
        _rigidbody.AddRelativeTorque(_torque, ForceMode.Impulse);
    }
}
