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
    }

    private IEnumerator RoamCoroutine()
    {
        _npcController.NavMeshAgent.speed = _npcController.RoamingSpeed;
        while (true)
        {
            if (_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
                _npcController.NavMeshAgent.SetDestination(newRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _npcController.RoamRadius;
        randomDirection += _npcController.RoamTargetLocation.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _npcController.RoamRadius, 1);
        return hit.position;
    }
}
