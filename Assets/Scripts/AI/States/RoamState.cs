using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : IState
{
    private readonly NpcController _npcController;
    private readonly AiDetection _aiDetection;
    private ObservableObject _currentRoamObject;

    public RoamState(NpcController npcController)
    {
        _npcController = npcController;
        _aiDetection = npcController.GetComponent<AiDetection>();
    }

    public void Handle()
    {
        _npcController.StartCoroutine(RoamCoroutine());
        _npcController.StartCoroutine(PeriodicallySetRoamOriginCoroutine());
    }

    private IEnumerator RoamCoroutine()
    {
        _npcController.Agent.stoppingDistance = 1f;
        _npcController.Agent.speed = _npcController.RoamingSpeed;

        while (IsRoaming())
        {
            if (_npcController.Agent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRandomCloseObjectPosition();
                Debug.Log("newRoamLocation: " + newRoamLocation);
                _npcController.Agent.SetDestination(newRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
    
    /// <summary>
    /// Sets the roam origin location every x seconds.
    /// </summary>
    private IEnumerator PeriodicallySetRoamOriginCoroutine()
    {
        while (IsRoaming())
        {
            _npcController.Agent.SetDestination(GetRandomCloseObjectPosition());
            yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 0.5f && !_npcController.Agent.pathPending);
            yield return new WaitForSeconds(_npcController.RoamOriginTimeSpent);
            _npcController.SetRoamOrigin();
        }
    }

    /// <summary>
    /// Returns the next roam waypoint position.
    /// </summary>
    public Vector3 GetNextRoamWaypointPosition()
    {
        _npcController.CurrentRoamIndex = (_npcController.CurrentRoamIndex + 1) % _npcController.CurrentRoamOrigin.transform.childCount;
        return _npcController.CurrentRoamOrigin.transform.GetChild(_npcController.CurrentRoamIndex).position;
    }

    public Vector3 GetRandomCloseObjectPosition()
    {
        Debug.Log("GetRandomCloseObjectPosition");
        List<ObservableObject> observableObjects = new(_aiDetection.DetectedObjects.Keys);
        observableObjects.Remove(_currentRoamObject);
        if(observableObjects.Count == 0)
        {
            return GetRoamLocation();
        }
        observableObjects.Sort((x, y) =>
        Vector3.Distance(_npcController.transform.position, x.transform.position).CompareTo(Vector3.Distance(_npcController.transform.position, y.transform.position)));

        return observableObjects[Random.Range(0, observableObjects.Count)].transform.position;
    }

    /// <summary>
    /// Returns a random location within the roam radius around the roam origin.
    /// </summary>
    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _npcController.RoamRadius;
        randomDirection += _npcController.CurrentRoamOrigin.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _npcController.RoamRadius, 1);
        return hit.position;
    }


    private bool IsRoaming()
    {
        return _npcController.CurrentState is RoamState;
    }
}
