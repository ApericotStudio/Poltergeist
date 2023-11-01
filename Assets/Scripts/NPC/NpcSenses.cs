using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The NPC's senses. Handles the NPC's field of view, hearing radius, and detection radius.
/// </summary>
public class NpcSenses : MonoBehaviour
{
    [Header("Sight Settings")]
    [Range(0, 360)]
    [Tooltip("The angle of the NPC's field of view.")]
    public float FieldOfViewAngle = 110f;
    [Tooltip("The distance that the NPC can see.")]
    [Range(0, 10)]
    public float SightRange = 10f;

    [Header("Auditory Settings")]
    [Tooltip("The distance that the NPC can hear.")]
    [Range(0, 20)]
    public float AuditoryRange = 10f;

    [Header("Senses Layers")]
    [Tooltip("The target layers that the NPC can see and hear.")]
    public LayerMask TargetMask;
    [Tooltip("The obstacle layers that block the NPC's vision.")]
    public LayerMask ObstacleMask;

    private float _detectionRange;
    public float DetectionRange 
    { 
        get {
            _detectionRange = Math.Max(AuditoryRange, SightRange);
            return _detectionRange;
        }
    }
    private NpcController _npcController;
    private List<Clutter> _detectedClutter = new();
    public List<Clutter> DetectedClutter { get => _detectedClutter; set => _detectedClutter = value; }

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        StartCoroutine (DetectTargetsWithDelay(.2f));
        _detectionRange = Math.Max(AuditoryRange, SightRange);
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

    private void Update()
    {
        ReactToClutterState();
    }
    /// <summary>
    /// Detects all targets in the NPC's detection radius.
    /// </summary>
    private void DetectTargets()
    {
        ClearDetectedClutter();

        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(transform.position, DetectionRange, TargetMask);
        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            Transform target = targetsInDetectionRadius[i].transform;
            if (target.TryGetComponent<Clutter>(out var clutter))
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    return;
                }
                if (Vector3.Angle (transform.forward, dirToTarget) < FieldOfViewAngle / 2) {
                    clutter.IsVisible = true;
                    DetectedClutter.Add(clutter);
                    return;
                }
                if (!(dstToTarget <= AuditoryRange))
                    return;
                clutter.IsAudible = true;
                DetectedClutter.Add(clutter);
            }
        }
        
    }
    /// <summary>
    /// Reacts to the state of the detected clutter.
    /// </summary>
    private void ReactToClutterState()
    {
        foreach (Clutter clutter in DetectedClutter)
        {
            switch(clutter.State)
            {
                case ClutterState.Idle:
                    break;
                case ClutterState.Moving:
                    UpdateAnxietyValue(clutter, clutter.MoveAnxietyValue);
                    break;
                case ClutterState.Destroyed:
                    UpdateAnxietyValue(clutter, clutter.DestroyAnxietyValue);
                    break;
            }
        }
    }
    /// <summary>
    /// Updates the NPC's anxiety value based on the state of the detected clutter.
    /// </summary>
    /// <param name="clutter">The detected clutter.</param>
    /// <param name="value">The amount of anxiety to be added.</param>
    private void UpdateAnxietyValue(Clutter clutter, float value)
    {
        if (clutter.IsVisible)
        {
            _npcController.AnxietyValue += value * clutter.VisualMultiplier * Time.deltaTime;
        }
        else if (clutter.IsAudible)
        {
            _npcController.AnxietyValue += value * clutter.AuditoryMultiplier * Time.deltaTime;
        }
    }
    /// <summary>
    /// Clears the detected clutter, also setting their visibility and audibility to false.
    /// </summary>
    private void ClearDetectedClutter()
    {
        foreach (Clutter clutter in DetectedClutter)
        {
            clutter.IsVisible = false;
            clutter.IsAudible = false;
        }
        DetectedClutter.Clear();
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
