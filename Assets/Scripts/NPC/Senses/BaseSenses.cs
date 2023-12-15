using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These properties are used to determine if the detected object is visible and/or audible.
/// </summary>
public class DetectedProperties
{
    public bool IsAudible = false;
    public bool IsVisible = false;
    public float DistanceToTarget;
}
/// <summary>
/// The base senses class is used to detect observable objebts. It functions as the base for all NPC senses.
/// </summary>
public abstract class BaseSenses : MonoBehaviour, IObserver
{
    [Header("NPC Senses Settings")]
    [Tooltip("The transform that the entity uses to detect targets."), SerializeField]
    public Transform _headTransform;
    [Tooltip("The target layers that the entity detects."), SerializeField]
    protected LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the entity's vision."), SerializeField]
    protected LayerMask _obstacleMask;
    [Header("Sight Settings")]
    [Tooltip("The angle of the AI's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the AI can see."), Range(0, 50)]
    public float SightRange = 20f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the AI can hear."), Range(0, 50)]
    public float AuditoryRange = 15f;
    
    [HideInInspector]
    public Dictionary<ObservableObject, DetectedProperties> DetectedObjects = new();
    [HideInInspector]
    public List<VisitorController> DetectedNpcs = new();

    public float DetectionRange { get { return Math.Max(AuditoryRange, SightRange); } }

    protected const float _detectionDelay = .2f;

    protected virtual void Awake()
    {
        StartCoroutine(DetectTargetsWithDelay(_detectionDelay));
    }

    protected IEnumerator DetectTargetsWithDelay(float delay)
    {
        while (true)
        {
            HandleTargets(
                DetectTargetsInSphere(DetectionRange, _targetMask)
                );
            yield return new WaitForSeconds(delay);
        }
    }

    protected Collider[] DetectTargetsInSphere(float range, LayerMask targetMask)
    {
        return Physics.OverlapSphere(transform.position, range, targetMask);
    }

    protected virtual void HandleTargets(Collider[] targetsInDetectionRadius)
    {
        ClearDetectedTargets();

        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            bool isNpc = target.TryGetComponent<VisitorController>(out var npc);

            if(isNpc)
            {
                DetectedNpcs.Add(npc);
                continue;
            }

            bool isObservableObject = target.TryGetComponent<ObservableObject>(out var observableObject);

            if (!isObservableObject)
                continue;
            
            DetectedProperties detectedProperties = new();

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;

            if (TargetInSightRadius(target))
            {
                detectedProperties.IsVisible = true;
            }

            if (distanceToTarget <= AuditoryRange)
            {
                detectedProperties.IsAudible = true;
            }
            detectedProperties.DistanceToTarget = distanceToTarget;
            DetectedObjects.Add(observableObject, detectedProperties);
            observableObject.AddObserver(this);
        }
    }

    protected virtual void ClearDetectedTargets()
    {
        foreach (ObservableObject detectedObject in DetectedObjects.Keys)
        {
            detectedObject.RemoveObserver(this);
        }
        DetectedObjects.Clear();
        DetectedNpcs.Clear();
    }

    protected bool TargetInSightRadius(Collider target)
    {
        Vector3 directionToTarget = (target.transform.position - _headTransform.position).normalized;
        float distanceToTarget = Vector3.Distance(_headTransform.position, target.ClosestPoint(_headTransform.position));
        return Vector3.Angle(_headTransform.forward, directionToTarget) < FieldOfViewAngle / 2 && distanceToTarget <= SightRange;
    }
    
    /// <summary>
    /// Returns a vector3 direction from an angle. Used for the field of view.
    /// </summary>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += _headTransform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

    public abstract void OnNotify(ObservableObject observableObject);
}