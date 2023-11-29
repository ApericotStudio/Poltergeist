using System.Collections.Generic;
using UnityEngine;

public class ObservablePhysics : MonoBehaviour
{
    private ObservableObject _observableObject;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private LayerMask _obstacleMask;
    private Collider _collider;
    [Tooltip("Is the object breakable?"), SerializeField]
    private bool _isBreakable = false;
    [Tooltip("Minimum Impulse needed to destroy the object"), SerializeField] 
    private float _destroyMinimumImpulse = 10;
    [Tooltip("Minimum Impulse needed for hitting ground sound"), SerializeField]
    private float _hitGroundSoundMinimumImpulse = 3;

    [HideInInspector]
    public bool FirstHit = true;

    [Header("Sound clips")]
    [SerializeField] private List<AudioClip> _hittingGroundSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _breakingSounds = new List<AudioClip>();

    private void Awake()
    {
        _observableObject = GetComponent<ObservableObject>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_observableObject.State == ObjectState.Broken)
        {
            return;
        }
        if (!FirstHit)
        {
            if (collision.gameObject.layer == _obstacleMask)
            {
                _observableObject.State = ObjectState.Hit;
            }
        }
        else
        {
            FirstHit = false;
        }
        if(_isBreakable)
        {
            if(collision.impulse.magnitude > _destroyMinimumImpulse)
            {
                _observableObject.State = ObjectState.Broken;

                PlayBreakingSound();

                bool isHighlightable = _observableObject.TryGetComponent(out Highlight highlight);
                if(isHighlightable)
                {
                    highlight.Highlightable(false);
                }
                
                _observableObject.ClearObservers();
            }
            else if(collision.impulse.magnitude > _hitGroundSoundMinimumImpulse)
            {
                PlayHittingGroundSound();
            }
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
            if(_observableObject.State != ObjectState.Idle)
            {
                _observableObject.State = ObjectState.Idle;
            }
        }
    }

    private bool IsObjectGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, _collider.bounds.extents.y + 0.1f);
    }
    
    private void PlayHittingGroundSound()
    {
        if (!(_hittingGroundSounds.Count > 0))
        {
            return;
        }
        int indexToPlay = Random.Range(0, _hittingGroundSounds.Count);
        _audioSource.clip = _hittingGroundSounds[indexToPlay];
        _audioSource.Play();
    }

    private void PlayBreakingSound()
    {
        if (!(_breakingSounds.Count > 0))
        {
            return;
        }
        int indexToPlay = Random.Range(0, _breakingSounds.Count);
        _audioSource.clip = _breakingSounds[indexToPlay];
        _audioSource.Play();
    }
}
