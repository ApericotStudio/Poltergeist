using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtorSenses : AiDetection, IObserver
{
    [Tooltip("The distance that the realtor can reduce NPC fear."), Range(0, 50)]
    public float FearReductionRange = 10f;
    
    [Header("Fear Reduction Settings")]
    [Tooltip("The value that will be subtracted from the fear value of npc's close to the realtor."), Range(0f, 1f), SerializeField]
    private float _reductionValue = 0.1f;
    [Tooltip("The speed at which the fear value will be reduced."), Range(0f, 1f), SerializeField]
    private float _reductionSpeed = 0.05f;
    
    [HideInInspector]
    public List<NpcController> DetectedNpcs = new();

    private RealtorController _realtorController;

    protected override void Awake()
    {
        base.Awake();
        _realtorController = GetComponent<RealtorController>();
        StartCoroutine(DecreaseNpcFear());
    }

    protected override void DetectTargets()
    {
        ClearDetectedObjects();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            bool isNpc = target.TryGetComponent<NpcController>(out var npc);
            bool isObservableObject = target.TryGetComponent<ObservableObject>(out var observableObject);

            if (!isNpc && !isObservableObject)
                continue;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;

            if (isNpc)
            {
                if (distanceToTarget <= FearReductionRange)
                {
                    DetectedNpcs.Add(npc);
                    npc.SeenByRealtor = true;
                }
            }

            if(isObservableObject)
            {
                DetectedProperties detectedProperties = new();

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
    }
    
    public void OnNotify(ObservableObject observableObject)
    {
        if (!DetectedObjects.TryGetValue(observableObject, out _))
            return;
        switch (observableObject.State)
        {
            case ObjectState.Interacted:
                _realtorController.InvestigateTarget = observableObject.transform;
                _realtorController.Investigate();
                break;
            case ObjectState.Hit:
                if (observableObject.Type == ObjectType.Small)
                {
                    _realtorController.InvestigateTarget = observableObject.transform;
                    _realtorController.Investigate();
                }
                break;
            default:
                return;
            }
    }
    
    protected override void ClearDetectedObjects()
    {
        foreach(NpcController npc in DetectedNpcs)
        {
            npc.SeenByRealtor = false;
        }
        DetectedNpcs.Clear();
        DetectedObjects.Clear();
    }

    private IEnumerator DecreaseNpcFear()
    {
        while (true)
        {
            if (DetectedNpcs.Count == 0)
            {
                yield return new WaitForSeconds(_reductionSpeed);
                continue;
            }
            foreach (NpcController npc in DetectedNpcs)
            {
                npc.FearValue -= _reductionValue;
            }
            yield return new WaitForSeconds(_reductionSpeed);
        }
    }
}
