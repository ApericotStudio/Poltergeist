using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : INpcState
{
    private readonly NpcController _npcController;
    
    public RoamState(NpcController npcController)
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
        _npcController.NavMeshAgent.stoppingDistance = 0f;
        _npcController.NavMeshAgent.speed = _npcController.RoamingSpeed;

        while (IsRoaming())
        {
            if (_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
                _npcController.NavMeshAgent.SetDestination(newRoamLocation);
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
            _npcController.NavMeshAgent.SetDestination(GetRoamLocation());
            yield return new WaitUntil(() => _npcController.NavMeshAgent.remainingDistance < 0.5f && !_npcController.NavMeshAgent.pathPending);
            yield return new WaitForSeconds(_npcController.RoamOriginTimeSpent);
            _npcController.SetRoamOrigin();
        }
    }

    private bool IsRoaming()
    {
        return _npcController.CurrentState is RoamState;
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
}
