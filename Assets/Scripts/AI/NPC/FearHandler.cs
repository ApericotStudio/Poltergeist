using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the fear value of the NPC and their reaction.
/// </summary>
public class FearHandler : MonoBehaviour
{
    public delegate void ObjectUsed(ObservableObject observableObject);
    public event ObjectUsed OnObjectUsed;

    [Header("Fear Handler Settings")]
    [Tooltip("The cooldown between each scare."), Range(0f, 10f), SerializeField]
    private float _scaredCooldown = 2f;

    [Header("Multipliers")]
    [Tooltip("Multiplier to scare value when an object is visible to an NPC."), Range(0f, 5f), SerializeField]
    private float _visibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an object is audible to an NPC."), Range(0f, 5f), SerializeField]
    private float _audibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an object is both visible and audible to an NPC."), Range(0f, 5f), SerializeField]
    private float _visibleAndAudibleMultiplier = 1.5f;
    [Tooltip("Amount fear goes up when object breaks"), SerializeField]
    private float _brokenAddition = 5f;
    [Tooltip("These multipliers are used to decrease the fear value as an object is used more frequently."), SerializeField]
    private List<float> _usageMultipliers = new() { 1f, 0.5f, 0.25f, 0.1f };
    [Tooltip("Amount of fear gets added when NPC's phobia triggers"), SerializeField]
    private float _phobiaValue = 10f;
    [Tooltip("Amount of added fear needed for NPC to get scared"), SerializeField]
    private float _scaredThreshold = 22f;

    private NpcController _npcController;
    private NpcSenses _npcSenses;
    private bool _isScared = false;
    private IEnumerator _coroutine;

    private readonly List<ObservableObject> _usedObjects = new();

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        _npcSenses = GetComponent<NpcSenses>();
    }
    
    /// <summary>
    /// Handles the fear value of the NPC and their reaction.
    /// </summary>
    /// <param name="observableObject">The object they've detected</param>
    /// <param name="detectedProperties">The object's detected properties</param>
    public void Handle(ObservableObject observableObject, DetectedProperties detectedProperties)
    {
        int objectUsageCount = _usedObjects.Count(x => x.Equals(observableObject));

        if(objectUsageCount >= _usageMultipliers.Count - 1)
        {
            objectUsageCount = _usageMultipliers.Count - 1;
        }

        if(_isScared)
        {
            return;
        }

       float fearToAdd = CalculateFearValue(observableObject, detectedProperties, objectUsageCount);

      if (_npcController.FearValue + fearToAdd < 100f)
        {
            if (observableObject.State != ObjectState.Hit && observableObject.State != ObjectState.Interacted)
            {
                return;
            }

            if (fearToAdd < _scaredThreshold)
            {
                _npcController.InvestigateTarget = observableObject.transform;
                _npcController.Investigate();
            }
            else
            {
                _npcController.GetScared();
            }
        }
       _npcController.FearValue += fearToAdd;
       _coroutine = ScaredCooldown();
       StartCoroutine(_coroutine);
       _usedObjects.Add(observableObject);
        OnObjectUsed?.Invoke(observableObject);
    }

    /// <summary>
    /// Calculates the fear value of the NPC based on the object's detected properties and usage count.
    /// </summary>
    /// <returns>The amount of fear it has calculated.</returns>
    private float CalculateFearValue(ObservableObject observableObject, DetectedProperties detectedProperties, int objectUsageCount)
    {
        float fearValue = detectedProperties switch
        {
            { IsVisible: true, IsAudible: true } => _visibleAndAudibleMultiplier,
            { IsAudible: true } => _audibleMultiplier,
            { IsVisible: true } => _visibleMultiplier,
            _ => 0
        };

        float falloff = detectedProperties switch
        {
            { IsVisible: true, IsAudible: true } => 0.8f - (detectedProperties.DistanceToTarget / _npcSenses.SightRange) * 0.8f + 0.2f,
            { IsAudible: true } => 0.8f - (detectedProperties.DistanceToTarget / _npcSenses.AuditoryRange) * 0.8f + 0.2f,
            { IsVisible: true } => 0.8f - (detectedProperties.DistanceToTarget / _npcSenses.SightRange) * 0.8f + 0.2f,
            _ => 0
        };

        float phobiaValue;
        if(observableObject.ObjectPhobia == _npcController.NPCPhobia && _npcController.NPCPhobia != ObjectPhobia.None)
        {
            phobiaValue = _phobiaValue;
        }
        else
        {
            phobiaValue = 0f;
        }

        float soothe;
        if (_npcController.SeenByRealtor)
        {
            soothe = 0.5f;
        }
        else
        {
            soothe = 1f;
        }

        float brokenAddition;
        if (observableObject.State == ObjectState.Broken)
        {
            brokenAddition = _brokenAddition;
        }
        else
        {
            brokenAddition = 0f;
        }

        return ((float)observableObject.Type * fearValue + brokenAddition + phobiaValue) * _usageMultipliers[objectUsageCount] * falloff * soothe;
    }

    private IEnumerator ScaredCooldown()
    {
        _isScared = true;
        yield return new WaitForSeconds(_scaredCooldown);
        _isScared = false;
    }
}
