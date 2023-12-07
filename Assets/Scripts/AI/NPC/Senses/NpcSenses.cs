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
    private FearHandler _fearHandler;

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
}
