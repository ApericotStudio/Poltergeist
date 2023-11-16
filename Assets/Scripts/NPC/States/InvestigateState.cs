using System.Collections;
using UnityEngine;

public class InvestigateState : INpcState
{
    private readonly NpcController _npcController;

    public InvestigateState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.NavMeshAgent.speed = _npcController.InvestigatingSpeed;
        _npcController.NavMeshAgent.stoppingDistance = 2f;
        _npcController.NavMeshAgent.SetDestination(_npcController.InvestigateTarget.position);
        _npcController.StartCoroutine(ReturnToRoamState());
    }

    IEnumerator ReturnToRoamState()
    {
        while (_npcController.NavMeshAgent.pathPending == true || _npcController.NavMeshAgent.remainingDistance > _npcController.NavMeshAgent.stoppingDistance)
        {
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        _npcController.CurrentState = _npcController.RoamState;
    }

}
