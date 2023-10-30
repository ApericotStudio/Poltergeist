using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : INpcState
{
    private readonly NpcController _npcController;
    private Coroutine _roamCoroutine;

    public RoamState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Execute()
    {
        if (_npcController.RoamTargetLocation == null)
        {
            return;
        }

        if (!_npcController.NavMeshAgent.pathPending && _npcController.NavMeshAgent.remainingDistance < 0.5f)
        {
            _roamCoroutine ??= _npcController.StartCoroutine(RoamCoroutine());
        }
    }

    private IEnumerator RoamCoroutine()
    {
        yield return new WaitForSeconds(2f);

        if (_npcController.CurrentState == this)
        {
            _npcController.NavMeshAgent.SetDestination(GetRandomRoamLocation());
            _roamCoroutine = null;
        }
    }

    private Vector3 GetRandomRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _npcController.RoamRadius;
        randomDirection += _npcController.RoamTargetLocation.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _npcController.RoamRadius, 1);
        return hit.position;
    }
}
