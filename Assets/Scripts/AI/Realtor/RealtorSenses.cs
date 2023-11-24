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
        DetectedNpcs.Clear();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Collider target = targetsInDetectionRadius[i];
            Debug.Log("Target detected");
            bool IsNpc = target.TryGetComponent<NpcController>(out var npc);

            if (!IsNpc)
                continue;
                Debug.Log("Npc detected");
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.ClosestPoint(transform.position));

            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                continue;

            DetectedNpcs.Add(npc);
        }
    }
}
