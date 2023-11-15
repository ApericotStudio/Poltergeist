using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : MonoBehaviour, IObserver
{
    [Header("Sight Settings")]
    [Tooltip("The angle of the NPC's field of view."), Range(0, 360)]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see."), Range(0, 10)]
    public float SightRange = 10f;
    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear."), Range(0, 20)]
    public float AuditoryRange = 10f;
    [Header("Senses Layers")]
    [Tooltip("The target layers that the NPC can see and hear."), SerializeField]
    private LayerMask _targetMask;
    [Tooltip("The obstacle layers that block the NPC's vision."), SerializeField]
    private LayerMask _obstacleMask;
    [Header("Reaction Settings")]
    [Tooltip("The delay between the NPC detecting a target and reacting to it."), Range(0f, 5f), SerializeField]
    private float _reactionDelay = 1f;
    [HideInInspector]
    public List<ObservableObject> DetectedObjects;
    public float DetectionRange { get { return Math.Max(AuditoryRange, SightRange); } }

    private void Awake()
    {
        StartCoroutine (DetectTargetsWithDelay(.2f));
    }

    /// <summary>
    /// Detects targets in the NPC's detection radius with a delay.
    /// </summary>
    /// <param name="delay">The delay between detecting.</param>
    /// <returns></returns>
	private IEnumerator DetectTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
            DetectTargets();
		}
	}

    /// <summary>
    /// Detects all targets in the NPC's detection radius.
    /// </summary>
    private void DetectTargets()
    {
        ClearDetectedObjects();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, _targetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Transform target = targetsInDetectionRadius[i].transform;
            if (target.TryGetComponent<ObservableObject>(out var clutter))
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                {
                    continue;
                }
                if (Vector3.Angle (transform.forward, dirToTarget) < FieldOfViewAngle / 2) {
                    clutter.IsVisible = true;
                }
                if (dstToTarget <= AuditoryRange)
                {
                    clutter.IsAudible = true;
                }
                DetectedObjects.Add(clutter);
                clutter.AddObserver(this);
            }
        }
    }

    public void OnNotify(ObservableObject observableObject)
    {

    }
    
    /// <summary>
    /// Clears the detected clutter, also setting their visibility and audibility to false.
    /// </summary>
    private void ClearDetectedObjects()
    {
        foreach (ObservableObject observableObject in DetectedObjects)
        {
            observableObject.IsVisible = false;
            observableObject.IsAudible = false;
            observableObject.RemoveObserver(this);
        }
        DetectedObjects.Clear();
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
