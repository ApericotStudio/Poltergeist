using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the fear value of the NPC and their reaction.
/// </summary>
public class FearHandler : MonoBehaviour
{
    [Header("Fear Handler Settings")]
    [Tooltip("The cooldown between each scare."), Range(0f, 10f), SerializeField]
    public float _scaredCooldown = 2f;

    [Header("Multipliers")]
    [Tooltip("Multiplier to scare value when an object is visible to an NPC."), Range(0f, 5f), SerializeField]
    private float _visibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an object is audible to an NPC."), Range(0f, 5f), SerializeField]
    private float _audibleMultiplier = 1f;
    [Tooltip("Multiplier to scare value when an object is both visible and audible to an NPC."), Range(0f, 5f), SerializeField]
    private float _visibleAndAudibleMultiplier = 1.5f;
    [Tooltip("These multipliers are used to decrease the fear value as an object is used more frequently."), SerializeField]
    private List<float> _usageMultipliers = new() { 1f, 0.5f, 0.25f, 0f};

    private NpcController _npcController;
    private bool _isScared = false;
    private IEnumerator _coroutine;

    private readonly List<ObservableObject> _usedObjects = new();

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
    }
    
    /// <summary>
    /// Handles the fear value of the NPC and their reaction.
    /// </summary>
    /// <param name="observableObject">The object they've detected</param>
    /// <param name="detectedProperties">The object's detected properties</param>
    public void Handle(ObservableObject observableObject, DetectedProperties detectedProperties)
    {
        _npcController.InvestigateTarget = observableObject.transform;

        int objectUsageCount = _usedObjects.Count(x => x.Equals(observableObject));

        if(objectUsageCount >= _usageMultipliers.Count - 1)
        {
            objectUsageCount = _usageMultipliers.Count - 1;
        }

        if(observableObject.State == ObjectState.Hit)
        {
            if(observableObject.Type == ObjectType.Small)
            {
                _npcController.Investigate();
            }
            else
            {
                _npcController.GetScared();
            }
        }
        if (observableObject.State == ObjectState.Interacted)
        {
            _npcController.Investigate();
        }

        if (observableObject.State == ObjectState.Idle || _isScared)
        {
            return;
        }

       _npcController.FearValue += CalculateFearValue(observableObject, detectedProperties, objectUsageCount);

        _coroutine = ScaredCooldown();
        StartCoroutine(_coroutine);
        _usedObjects.Add(observableObject);
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

        return (float)observableObject.Type * fearValue * _usageMultipliers[objectUsageCount];
    }

    private IEnumerator ScaredCooldown()
    {
        _isScared = true;
        yield return new WaitForSeconds(_scaredCooldown);
        _isScared = false;
    }
}
