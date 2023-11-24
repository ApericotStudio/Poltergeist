using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RealtorController : MonoBehaviour
{
    [Header("Realtor Settings")]
    [Tooltip("The speed the realtor will move when roaming."), Range(1f, 5f)]
    public float RoamingSpeed = 2f;
    [Tooltip("All the NPCs in the scene."), SerializeField]
    private List<Transform> _npcs;
    [Tooltip("The amount of time the NPC will stay around one npc origin"), Range(0f, 100f), SerializeField]
    private float _npcOriginTimeSpent = 30f;
    [Tooltip("The radius around the NPC the realtor will roam around in."), Range(1f, 10f), SerializeField]
    private float _npcOriginRadius = 5f;
    [Header("Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float FootstepVolume = 0.5f;

    private Transform _currentNpcOrigin;
    private float _animationBlend;
    private readonly int _animIDSpeed = Animator.StringToHash("Speed");
    private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    private Animator _animator;
    private int _currentNpcIndex = 0;


    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentNpcOrigin = _npcs[_currentNpcIndex];
        StartCoroutine(PeriodicallySetNpcOriginCoroutine());
        StartCoroutine(RoamAroundNpc());
    }

    private void Update()
    {
        Animate();
    }

    private IEnumerator PeriodicallySetNpcOriginCoroutine()
    {
        while(true)
        {
            _navMeshAgent.SetDestination(GetRoamLocation());
            yield return new WaitUntil(() => _navMeshAgent.remainingDistance < 1f && !_navMeshAgent.pathPending);
            yield return new WaitForSeconds(_npcOriginTimeSpent);
            _navMeshAgent.SetDestination(_currentNpcOrigin.position);
            _currentNpcOrigin = GetNewNpcOrigin();
        }
    }

    private IEnumerator RoamAroundNpc()
    {
        _navMeshAgent.stoppingDistance = 0f;
        _navMeshAgent.speed = RoamingSpeed;

        while (true)
        {
            if (_navMeshAgent.remainingDistance < 0.5f)
            {
                _navMeshAgent.SetDestination(GetRoamLocation());
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _npcOriginRadius;
        randomDirection += _currentNpcOrigin.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _npcOriginRadius, 1);
        return hit.position;
    }

    private Transform GetNewNpcOrigin()
    {
        _currentNpcIndex = (_currentNpcIndex + 1) % _npcs.Count;
        return _npcs[_currentNpcIndex];
    }
    
    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, _navMeshAgent.velocity.magnitude, Time.deltaTime * _navMeshAgent.acceleration);
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
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepVolume);
            }
        }
    }
}
