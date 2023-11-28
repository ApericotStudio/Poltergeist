using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtorSenses : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("The target layer the realtor detects."), SerializeField]
    private LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the realtor's vision."), SerializeField]
    private LayerMask _obstacleMask;
    [Tooltip("The distance that the realtor can detect."), Range(0, 50)]
    public float DetectionRange = 10f;
    [Header("Fear Reduction Settings")]
    [Tooltip("The value that will be subtracted from the fear value of npc's close to the realtor."), Range(0f, 1f), SerializeField]
    private float _reductionValue = 0.1f;
    [Tooltip("The speed at which the fear value will be reduced."), Range(0f, 1f), SerializeField]
    private float _reductionSpeed = 0.05f;
    
    private const float DetectionDelay = .2f;

    [HideInInspector]
    public List<NpcController> DetectedNpcs = new();

    private void Awake()
    {
        StartCoroutine(DetectNpcsWithDelay(DetectionDelay));
        StartCoroutine(DecreaseNpcFear());
    }

    private IEnumerator DetectNpcsWithDelay(float delay)
    {
        while (true)
        {
            DetectNpcs();
            yield return new WaitForSeconds(delay);
        }
    }

    private void DetectNpcs()
    {
        ClearDetectedNpcs();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            bool IsNpc = target.TryGetComponent<NpcController>(out var npc);

            if (!IsNpc)
                continue;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;

            DetectedNpcs.Add(npc);
        }
    }

    private void ClearDetectedNpcs()
    {
        DetectedNpcs.Clear();
    }

    IEnumerator DecreaseNpcFear()
    {
        while (true)
        {
            if(DetectedNpcs.Count == 0)
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
