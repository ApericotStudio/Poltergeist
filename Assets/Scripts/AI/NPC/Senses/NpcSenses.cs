using System;
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
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : AiDetection, IObserver
{
    [Header("Sight Settings")]
    [Tooltip("The angle of the NPC's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see."), Range(0, 50)]
    public float SightRange = 20f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear."), Range(0, 50)]
    public float AuditoryRange = 15f;

    public float DetectionRange { get { return Math.Max(AuditoryRange, SightRange); } }

    private FearHandler _fearHandler;
    public Dictionary<ObservableObject, DetectedProperties> DetectedObjects = new();

    protected override void Awake()
    {
        base.Awake();
        _fearHandler = GetComponent<FearHandler>();
    }

    protected override void DetectTargets()
    {
        ClearDetectedObjects();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            bool isObservableObject = target.TryGetComponent<ObservableObject>(out var observableObject);

            if (!isObservableObject)
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
            detectedProperties.DistanceToTarget = distanceToTarget;
            DetectedObjects.Add(observableObject, detectedProperties);
            observableObject.AddObserver(this);
        }
    }
    
    protected override void ClearDetectedObjects()
    {
        foreach (ObservableObject detectedObject in DetectedObjects.Keys)
        {
            detectedObject.RemoveObserver(this);
        }
        DetectedObjects.Clear();
    }


    public void OnNotify(ObservableObject observableObject)
    {
        if (!DetectedObjects.TryGetValue(observableObject, out var detectedProperties))
            return;
        
        _fearHandler.Handle(observableObject, detectedProperties);
    }

    private bool TargetInSightRadius(Vector3 directionToTarget, float distanceToTarget)
    {
        return Vector3.Angle(transform.forward, directionToTarget) < FieldOfViewAngle / 2 && distanceToTarget <= SightRange;
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
