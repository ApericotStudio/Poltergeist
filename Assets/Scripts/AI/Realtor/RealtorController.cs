using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RealtorController : AIController
{
    [Header("Realtor Settings")]
    [Tooltip("The speed the realtor will move when roaming."), Range(1f, 5f)]
    public float RoamingSpeed = 2f;
    [Tooltip("The GameObject that contains all the NPC's"), SerializeField]
    private GameObject _npcCollection;
    [Tooltip("The amount of time the realtor will check up on an NPC."), Range(0f, 100f)]
    public float CheckUpTimeSpent = 30f;
    [Tooltip("The radius around the NPC the realtor will roam around in."), Range(1f, 10f), SerializeField]
    public float CheckUpRadius = 5f;
    [Header("Audio Settings")]
    [Tooltip("The audio clips that will be played when the NPC moves.")]
    public AudioClip[] FootstepAudioClips;
    [Tooltip("The volume of the footstep audio clips.")]
    [Range(0f, 1f)]
    public float FootstepVolume = 0.5f;
    [Tooltip("The current check up origin of the realtor. This is the location the realtor will roam around.")]
    public Transform CurrentCheckUpOrigin;
    private float _animationBlend;
    private int _currentNpcIndex = 0;

    private CheckUpState _checkUpState;

    private void Awake()
    {
        InitializeController();
        CurrentCheckUpOrigin = _npcCollection.transform.GetChild(_currentNpcIndex);
        _checkUpState = new CheckUpState(this);
    }

    private void Start() {
        CurrentState = _checkUpState;
    }

    private void Update()
    {
        Animate();
    }

    public Transform SetCheckUpOrigin()
    {

        _currentNpcIndex = (_currentNpcIndex + 1) % _npcCollection.transform.childCount;
        return _npcCollection.transform.GetChild(_currentNpcIndex);
    }
    
    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, Agent.velocity.magnitude, Time.deltaTime * Agent.acceleration);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Animator.SetFloat(AnimIDSpeed, _animationBlend);
        Animator.SetFloat(AnimIDMotionSpeed, 1f);
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
