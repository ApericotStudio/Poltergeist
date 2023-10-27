using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    Roam,
    Frightened
}
public class NpcController : MonoBehaviour
{
    [Tooltip("The target location the NPC will roam around.")]
    public Transform RoamTargetLocation;
    [Tooltip("The radius around the Roam Target Location the NPC will in.")]
    [Range(0f, 10f)]
    public float RoamRadius = 5f;
    [Tooltip("The target location the NPC will run to when frightened.")]
    public Transform FrightenedTargetLocation;

    private NavMeshAgent _agent;
    private NpcState _currentState;
    private Coroutine _roamCoroutine;

    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private float _animationBlend;
    private Animator _animator;

    public AudioClip[] FootstepAudioClips;
    [Range(0f, 1f)]
    public float footstepVolume = 0.5f;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        InitializeAnimator();
    }

    private void Update()
    {
        Animate();
        ActOutState();
    }

    private void ActOutState()
    {
        switch (_currentState)
        {
            case NpcState.Roam:
                Roam();
                break;
            case NpcState.Frightened:
                Frightened();
                break;
        }
    }

    private void Roam()
    {
        if (_roamCoroutine == null)
        {
            _roamCoroutine = StartCoroutine(RoamCoroutine());
        }
    }

    private IEnumerator RoamCoroutine()
    {
        while (true)
        {
            if (_agent.remainingDistance < 0.5f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * RoamRadius;
                randomDirection += RoamTargetLocation.position;
                NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10f, NavMesh.AllAreas);
                _agent.SetDestination(hit.position);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    private void Frightened()
    {
        _agent.SetDestination(FrightenedTargetLocation.position);
    }

    private void InitializeAnimator()
    {
        _animator = GetComponent<Animator>();
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDSpeed = Animator.StringToHash("Speed");
    }
    
    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, _agent.velocity.magnitude, Time.deltaTime * _agent.acceleration);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                int index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, footstepVolume);
            }
        }
    }
}
