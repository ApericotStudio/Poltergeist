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
        _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateAudioClip);
        _npcController.StartCoroutine(InvestigateCoroutine());
    }

    /// <summary>
    /// Moves the NPC to the location of the object that made a sound and then back to the roam location.
    /// </summary>
    /// <returns></returns>
    IEnumerator InvestigateCoroutine()
    {
        while(_npcController.NavMeshAgent.pathPending || _npcController.NavMeshAgent.remainingDistance > _npcController.NavMeshAgent.stoppingDistance)
        {
            _npcController.NavMeshAgent.SetDestination(_npcController.InvestigateTarget.position);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        while (_npcController.NavMeshAgent.pathPending || _npcController.NavMeshAgent.remainingDistance > _npcController.NavMeshAgent.stoppingDistance)
        {
            yield return null;
        }
        _npcController.CurrentState = _npcController.RoamState;
        _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateEndAudioClip);
        _npcController.NavMeshAgent.SetDestination(_npcController.RoamTargetLocation.position);
        yield break;
    }
}
