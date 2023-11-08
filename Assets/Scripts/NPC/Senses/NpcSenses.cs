using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : MonoBehaviour, IObserver
{
    [Header("Sight Settings")]
    [Range(0, 360)]
    [Tooltip("The angle of the NPC's field of view.")]
    [SerializeField]
    private float _fieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see.")]
    [Range(0, 10)]
    [SerializeField]
    private float _sightRange = 10f;
    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear.")]
    [Range(0, 20)]
    [SerializeField]
    private float _auditoryRange = 10f;
    [Header("Senses Layers")]
    [Tooltip("The target layers that the NPC can see and hear.")]
    [SerializeField]
    private LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the NPC's vision.")]
    [SerializeField]
    private LayerMask _obstacleMask;
    [Header("Reaction Settings")]
    [SerializeField]
    [Tooltip("The delay between the NPC detecting a target and reacting to it.")]
    [Range(0f, 5f)]
    private float _reactionDelay = 1f;

    private float _detectionRange;
    public float DetectionRange 
    { 
        get {
            _detectionRange = Math.Max(_auditoryRange, _sightRange);
            return _detectionRange;
        }
    }
    private NpcController _npcController;
    private List<ObservableObject> _detectedObjects = new();
    public List<ObservableObject> DetectedObjects { get => _detectedObjects; set => _detectedObjects = value; }
    public float SightRange{ get => _sightRange; set => _sightRange = value; }
    public float FieldOfViewAngle { get => _fieldOfViewAngle; set => _fieldOfViewAngle = value; }
    public float AuditoryRange { get => _auditoryRange; set => _auditoryRange = value; }

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        StartCoroutine (DetectTargetsWithDelay(.2f));
        _detectionRange = Math.Max(_auditoryRange, _sightRange);
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
            Transform target = targetsInDetectionRadius[i].transform;
            if (target.TryGetComponent<ObservableObject>(out var clutter))
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                {
                    return;
                }
                if (Vector3.Angle (transform.forward, dirToTarget) < _fieldOfViewAngle / 2) {
                    clutter.IsVisible = true;
                    DetectedObjects.Add(clutter);
                    return;
                }
                if (!(dstToTarget <= _auditoryRange))
                    return;
                clutter.IsAudible = true;
                DetectedObjects.Add(clutter);
                clutter.AddObserver(this);
            }
        }
    }
    public void OnNotify(ObservableObject observableObject)
    {

    }

    // IEnumerator ReactionCoroutine(ObservableObject observableObject)
    // {
    //     yield return new WaitForSeconds(_reactionDelay);
    //     while(observableObject.State == ObjectState.Falling || observableObject.State == ObjectState.Destroyed)
    //     {
    //         CalculateFear(observableObject);
    //     }
    //     yield break;
    // }

    // private void CalculateFear(ObservableObject observableObject)
    // {
    //     float fearValueToAdd = 0f;
    //     if (observableObject.IsVisible)
    //     {
    //         fearValueToAdd += observableObject.VisualAnxietyValue;
    //     }
    //     if (observableObject.IsAudible)
    //     {
    //         fearValueToAdd += observableObject.AuditoryAnxietyValue;
    //     }
    //     if (observableObject.Type == ObjectType.Big)
    //     {
    //         fearValueToAdd += observableObject.BigObjectAnxietyValue;
    //     }
    //     if (observableObject.Type == ObjectType.Small)
    //     {
    //         fearValueToAdd += observableObject.SmallObjectAnxietyValue;
    //     }
    //     if (observableObject.State == ObjectState.Destroyed)
    //     {
    //         fearValueToAdd += observableObject.DestroyAnxietyValue;
    //     }
    //     if (observableObject.State == ObjectState.Falling && observableObject.IsVisible)
    //     {
    //         fearValueToAdd += observableObject.MoveAnxietyValue;
    //     }
    //     _npcController.FearValue += fearValueToAdd;
    // }

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
}
