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
        if(_npcController.NavMeshAgent.speed != _npcController.RoamingSpeed)
        {
            _npcController.NavMeshAgent.speed = _npcController.RoamingSpeed;
        }

        if(_npcController.AnxietyValue == 100f)
        {
            _npcController.StopCoroutine(_roamCoroutine);
            _npcController.CurrentState = new PanickedState(_npcController);
            return;
        }
        
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
        while (true)
        {
            if (_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                Vector3 randomRoamLocation = GetRandomRoamLocation();
                _npcController.NavMeshAgent.SetDestination(randomRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
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
