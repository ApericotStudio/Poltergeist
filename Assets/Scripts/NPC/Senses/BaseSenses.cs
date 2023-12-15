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
    [Tooltip("The transform that the entity uses to detect targets.")]
    public Transform HeadTransform;
    [Tooltip("The target layers that the entity detects."), SerializeField]
    protected LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the entity's vision."), SerializeField]
    protected LayerMask _obstacleMask;
    [Header("Sight Settings")]
    [Tooltip("The angle of the npc's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the npc can see."), Range(0, 50)]
    public float SightRange = 20f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the npc can hear."), Range(0, 50)]
    public float AuditoryRange = 15f;
    
    [HideInInspector]
    public Dictionary<ObservableObject, DetectedProperties> DetectedObjects = new();
    [HideInInspector]
    public List<VisitorController> DetectedVisitors = new();

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

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;
                
            bool isVisitor = target.TryGetComponent<VisitorController>(out var visitor);

            if(isVisitor)
            {
                DetectedVisitors.Add(visitor);
                continue;
            }

            bool isObservableObject = target.TryGetComponent<ObservableObject>(out var observableObject);

            if (!isObservableObject)
                continue;
            
            DetectedProperties detectedProperties = new();

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
        DetectedVisitors.Clear();
    }

    protected bool TargetInSightRadius(Collider target)
    {
        Vector3 directionToTarget = (target.transform.position - HeadTransform.position).normalized;
        float distanceToTarget = Vector3.Distance(HeadTransform.position, target.ClosestPoint(HeadTransform.position));
        return Vector3.Angle(HeadTransform.forward, directionToTarget) < FieldOfViewAngle / 2 && distanceToTarget <= SightRange;
    }

    public abstract void OnNotify(ObservableObject observableObject);
}