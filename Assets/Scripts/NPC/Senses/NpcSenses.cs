using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// These properties are used to determine if the detected object is visible and/or audible.
/// </summary>
public class DetectedProperties
{
    public bool IsAudible = false;
    public bool IsVisible = false;
}

/// <summary>
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : MonoBehaviour, IObserver
{
    [Header("Sight Settings")]
    [Tooltip("The angle of the NPC's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see."), Range(0, 50)]
    public float SightRange = 20f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear."), Range(0, 50)]
    public float AuditoryRange = 15f;

    [Header("Senses Layers")]
    [Tooltip("The target layers that the NPC can see and hear."), SerializeField]
    private LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the NPC's vision."), SerializeField]
    private LayerMask _obstacleMask;

    [Header("Reaction Settings")]
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
    public Dictionary<ObservableObject, DetectedProperties> DetectedObjects = new();
    public float DetectionRange { get { return Math.Max(AuditoryRange, SightRange); } }

    private NpcController _npcController;
    private bool _isScared = false;

    private IEnumerator _coroutine;

    private const float DetectionDelay = .2f;

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        StartCoroutine (DetectTargetsWithDelay(DetectionDelay));
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
            bool IsObservableObject = target.TryGetComponent<ObservableObject>(out var observableObject);

            if (!IsObservableObject)
                continue;
            
            DetectedProperties detectedProperties = new();

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;

            if (TargetInSightRadius(directionToTarget, distanceToTarget))
            {
                detectedProperties.IsVisible = true;
            }

            if (distanceToTarget <= AuditoryRange)
            {
                detectedProperties.IsAudible = true;
            }
            DetectedObjects.Add(observableObject, detectedProperties);
            observableObject.AddObserver(this);
        }
    }

    private bool TargetInSightRadius(Vector3 directionToTarget, float distanceToTarget)
    {
        if (Vector3.Angle(transform.forward, directionToTarget) < FieldOfViewAngle / 2 && distanceToTarget <= SightRange) 
        {
            return true;
        }
        return false;
    }

    public void OnNotify(ObservableObject observableObject)
    {
        if (!DetectedObjects.TryGetValue(observableObject, out var detectedProperties))
            return;

        _npcController.InvestigateTarget = observableObject.transform;

        bool audible = detectedProperties.IsAudible;
        bool visible = detectedProperties.IsVisible;

        int amountObject = _npcController._usedObjects.Count(x => x.Equals(observableObject));


        if(amountObject >= _usageMultipliers.Count - 1)
        {
            amountObject = _usageMultipliers.Count - 1;
        }
        if(observableObject.State == ObjectState.Hit)
        {
            if(observableObject.Type == ObjectType.Small)
            {
                Investigate();
            }
            else
            {
                GetScared();
            }
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
        foreach (ObservableObject detectedObject in DetectedObjects.Keys)
        {
            detectedObject.RemoveObserver(this);
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
        if(_npcController.CurrentState is not InvestigateState and not PanickedState && _npcController.FearValue < 100f)
        {
            _npcController.CurrentState = _npcController.InvestigateState;
        }
    }

    private void GetScared()
    {
        if(_npcController.CurrentState is not ScaredState and not PanickedState && _npcController.FearValue < 100f)
        {
            _npcController.CurrentState = _npcController.ScaredState;
        }
    }
}
