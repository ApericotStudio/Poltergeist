using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : MonoBehaviour, IObserver
{
    [Header("Sight Settings")]
    [Tooltip("The angle of the NPC's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see."), Range(0, 10)]
    public float SightRange = 10f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear."), Range(0, 20)]
    public float AuditoryRange = 10f;

    [Header("Senses Layers")]
    [Tooltip("The target layers that the NPC can see and hear."), SerializeField]
    private LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the NPC's vision."), SerializeField]
    private LayerMask _obstacleMask;

    [Header("Reaction Settings")]
    [Tooltip("The delay between the NPC detecting a target and reacting to it."), Range(0f, 5f), SerializeField]
    private float _reactionDelay = 1f;
    [Tooltip("Amount of time cooldown applies to NPC scare"), Range(0f, 10f), SerializeField]
    private float _scaredCooldown = 2f;

    [Header("Multipliers")]
    [Tooltip("Multiplier to scare value when an NPC hears a ghost."), Range(0f, 5f), SerializeField]
    private float _visibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an NPC sees a ghost."), Range(0f, 5f), SerializeField]
    private float _audibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an NPC sees and hears a ghost."), Range(0f, 5f), SerializeField]
    private float _AudibleAndVisibleMultiplier = 1.5f;
    [Tooltip("Amount object gets less scary after usage"), SerializeField]

    private List<float> _usageMultipliers = new() { 1f, 0.5f, 0.25f, 0f};
    [HideInInspector]
    public List<ObservableObject> DetectedObjects;
    public float DetectionRange { get { return Math.Max(AuditoryRange, SightRange); } }

    private NpcController _npcController;
    public AudioClip ScaredAudio;
    private bool _hasScreamed;
    private bool _isScared = false;

    private IEnumerator _coroutine;

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        StartCoroutine (DetectTargetsWithDelay(.2f));
    }

    /// <summary>
    /// Detects targets in the NPC's detection radius with a delay.
    /// </summary>
    /// <param name="delay">The delay between detecting.</param>
    /// <returns></returns>
	private IEnumerator DetectTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
            DetectTargets();
		}
	}

    /// <summary>
    /// Detects all targets in the NPC's detection radius.
    /// </summary>
    private void DetectTargets()
    {
        ClearDetectedObjects();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            if (target.TryGetComponent<ObservableObject>(out var observableObject))
            {
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                float dstToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                {
                    continue;
                }
                if (Vector3.Angle (transform.forward, dirToTarget) < FieldOfViewAngle / 2 && dstToTarget <= SightRange) {
                    observableObject.IsVisible = true;
                }
                if (dstToTarget <= AuditoryRange)
                {
                    observableObject.IsAudible = true;
                }
                DetectedObjects.Add(observableObject);
                observableObject.AddObserver(this);
            }
        }
    }

    public void OnNotify(ObservableObject observableObject)
    {
        _npcController.InvestigateTarget = observableObject.transform;

        bool audible = observableObject.IsAudible;
        bool visible = observableObject.IsVisible;

        int amountObject = _npcController._usedObjects.Count(x => x.Equals(observableObject));


        if(amountObject >= _usageMultipliers.Count - 1)
        {
            amountObject = _usageMultipliers.Count - 1;
        }
        if(observableObject.State == ObjectState.Hit)
        {
            Investigate();
        }
        if(observableObject.State == ObjectState.Idle || _isScared)
        {
            return;
        }

        if (audible && visible)
        {
            _npcController.FearValue += (float)observableObject.Type * _AudibleAndVisibleMultiplier * _usageMultipliers[amountObject];
        }

        else if (audible)
        {
            _npcController.FearValue += (float)observableObject.Type * _audibleMultiplier * _usageMultipliers[amountObject];
            if (!_hasScreamed)
            {
                _hasScreamed = true;
                AudioSource.PlayClipAtPoint(ScaredAudio, transform.position);
            }
        }

        else if (visible)
        {
            _npcController.FearValue += (float)observableObject.Type * _visibleMultiplier * _usageMultipliers[amountObject];
        }

        else
        {
            return;
        }

        _coroutine = ScaredCooldown();
        StartCoroutine(_coroutine);
        _npcController._usedObjects.Add(observableObject);
    }

    private IEnumerator ScaredCooldown()
    {
        _isScared = true;
        yield return new WaitForSeconds(_scaredCooldown);
        _isScared = false;
    }

    /// <summary>
    /// Clears the detected clutter, also setting their visibility and audibility to false.
    /// </summary>
    private void ClearDetectedObjects()
    {
        foreach (ObservableObject observableObject in DetectedObjects)
        {
            observableObject.IsVisible = false;
            observableObject.IsAudible = false;
            observableObject.RemoveObserver(this);
        }
        DetectedObjects.Clear();
    }

    /// <summary>
    /// Returns a vector3 direction from an angle. Used for the field of view.
    /// </summary>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

    private void Investigate()
    {
        if(_npcController.CurrentState != _npcController.InvestigateState)
        {
            _npcController.CurrentState = _npcController.InvestigateState;
        }
    }
}
