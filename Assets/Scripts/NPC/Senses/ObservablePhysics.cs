using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObservablePhysics : MonoBehaviour
{
    private ObservableObject _observableObject;
    private Rigidbody _rigidbody;
    private LayerMask _obstacleMask;
    private Collider _collider;
    [SerializeField]
    private bool _isBreakable = false;
    [SerializeField] 
    [Tooltip("Minimum Impulse needed to destroy the object")] 
    private float _destroyMinimumImpulse = 10;

    private bool _firstHit = true;

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_observableObject.State == ObjectState.Broken)
        {
            return;
        }
        if (!_firstHit)
        {
            if (collision.gameObject.layer == _obstacleMask)
            {
                _observableObject.State = ObjectState.Hit;
            }
        }
        else
        {
            _firstHit = false;
        }

        if(collision.impulse.magnitude > _destroyMinimumImpulse)
        {
            _observableObject.State = ObjectState.Broken;
        }
    }
        
    private void OnCollisionExit(Collision collision)
    {
        if(_observableObject.State == ObjectState.Broken)
        {
            return;
        }
        if (collision.gameObject.layer == _obstacleMask)
        {
            _observableObject.State = ObjectState.Moving;
        }
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if(_observableObject.State == ObjectState.Broken)
        {
            return;
        }
        if (_rigidbody.velocity.magnitude > 0.1f)
        {
            _observableObject.State = ObjectState.Moving;
        }
        else
        {
            _observableObject.State = ObjectState.Idle;
        }
    }

    private bool IsObjectGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, _collider.bounds.extents.y + 0.1f);
    }
    
}
