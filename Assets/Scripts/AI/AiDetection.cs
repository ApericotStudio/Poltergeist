using System.Collections;
using UnityEngine;

/// <summary>
/// The base abstract class for AI detection.
/// </summary>
public abstract class AiDetection : MonoBehaviour
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

    protected abstract void DetectTargets();

    protected abstract void ClearDetectedObjects();
}