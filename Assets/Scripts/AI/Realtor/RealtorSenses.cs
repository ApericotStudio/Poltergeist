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
    private const float DetectionDelay = .2f;

    [HideInInspector]
    public List<NpcController> DetectedNpcs = new();

    private void Awake()
    {
        StartCoroutine(DetectNpcsWithDelay(DetectionDelay));
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
            ReduceNpcFear();
        }
    }

    private void ClearDetectedNpcs()
    {
        foreach (NpcController npc in DetectedNpcs)
        {
            npc.gameObject.GetComponent<FearHandler>().FearReductionEnabled = false;
        }
        DetectedNpcs.Clear();
    }

    private void ReduceNpcFear()
    {
        foreach (NpcController npc in DetectedNpcs)
        {
            npc.gameObject.GetComponent<FearHandler>().FearReductionEnabled = true;
        }
    }
}
