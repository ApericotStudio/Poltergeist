using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for AI detection.
/// </summary>
public class AiDetection : MonoBehaviour
{
    [Header("AI Detection Settings")]
    [Tooltip("The target layers that the entity detects."), SerializeField]
    protected LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the entity's vision."), SerializeField]
    protected LayerMask _obstacleMask;

    protected const float _detectionDelay = .2f;

    protected virtual void Awake()
    {
        StartCoroutine(DetectTargetsWithDelay(_detectionDelay));
    }

    protected IEnumerator DetectTargetsWithDelay(float delay)
    {
        while (true)
        {
            DetectTargets();
            yield return new WaitForSeconds(delay);
        }
    }

    protected virtual void DetectTargets() { }

    protected virtual void ClearDetectedObjects() { }
}