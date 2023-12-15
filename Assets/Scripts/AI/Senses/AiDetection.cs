using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base abstract class for AI detection.
/// </summary>
public abstract class AiDetection : MonoBehaviour
{
    [Header("AI Detection Settings")]
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
            DetectTargets();
            yield return new WaitForSeconds(delay);
        }
    }

    protected abstract void DetectTargets();

    protected abstract void ClearDetectedObjects();

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
}