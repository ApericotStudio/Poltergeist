using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Settings")]
    [Tooltip("The objects the NPC wants to inspect in this room."), SerializeField]
    private List<Transform> _inspectableObjects;

    public Transform GetRandomInspectableObject(Transform exclude)
    {
        List<Transform> inspectableObjectsExcludingCurrent = _inspectableObjects.Where(obj => obj != exclude).ToList();
        if (inspectableObjectsExcludingCurrent.Count == 0)
        {
            return _inspectableObjects.First();
        }
        Transform inspectable = inspectableObjectsExcludingCurrent[Random.Range(0, inspectableObjectsExcludingCurrent.Count)];
        return inspectable;
    }
}
