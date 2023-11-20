using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : INpcState
{
    private readonly NpcController _npcController;

    private int currentRoamIndex = 0;

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
        _npcController.NavMeshAgent.stoppingDistance = 0f;
        _npcController.NavMeshAgent.speed = _npcController.RoamingSpeed;
        while (true)
        {
            if(_npcController.CurrentState is not RoamState)
                yield break;
        
            if (_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
                _npcController.NavMeshAgent.SetDestination(newRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    private IEnumerator ChooseRoamTargetLocationCoroutine()
    {
        while (true)
        {
            if(_npcController.CurrentState is not RoamState)
                yield break;
        
            ChooseRoamTargetLocation();
            yield return new WaitForSeconds(_npcController.TimeSpentInOneRoamLocation);
        }
    }

    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _npcController.RoamRadius;
        randomDirection += _npcController.CurrentRoamTargetLocation.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _npcController.RoamRadius, 1);
        return hit.position;
    }

    private void ChooseRoamTargetLocation()
    {
        currentRoamIndex++;
        
        if (currentRoamIndex == _npcController.RoamTargetLocations.Length)
        {
            currentRoamIndex = 0;
        }

        _npcController.CurrentRoamTargetLocation = _npcController.RoamTargetLocations[currentRoamIndex];
    }
}
