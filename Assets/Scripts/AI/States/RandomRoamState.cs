using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoamState : IState
{
    private readonly NpcController _npcController;
    
    public RandomRoamState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(RoamCoroutine());
        _npcController.StartCoroutine(PeriodicallySetRoamOriginCoroutine());
    }

    private IEnumerator RoamCoroutine()
    {
        _npcController.Agent.stoppingDistance = 0f;
        _npcController.Agent.speed = _npcController.RoamingSpeed;

        while (IsRoaming())
        {
            if (_npcController.Agent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
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
            _npcController.Agent.SetDestination(GetRoamLocation());
            yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 0.5f && !_npcController.Agent.pathPending);
            yield return new WaitForSeconds(_npcController.RoamOriginTimeSpent);
            _npcController.SetRoamOrigin();
        }
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
        return _npcController.CurrentState is RandomRoamState;
    }
}
