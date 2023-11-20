using System.Collections;
using UnityEngine;

public class InvestigateState : INpcState
{
    private readonly NpcController _npcController;

    private Collider _investigateTargetCollider;

    public InvestigateState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _investigateTargetCollider = _npcController.InvestigateTarget.GetComponent<Collider>();
        _npcController.StartCoroutine(InvestigateCoroutine());
    }

    /// <summary>
    /// Moves the NPC to the location of the object that made a sound and then back to the roam location.
    /// </summary>
    /// <returns></returns>
    IEnumerator InvestigateCoroutine()
    {
        _npcController.NavMeshAgent.speed = _npcController.InvestigatingSpeed;
        _npcController.NavMeshAgent.stoppingDistance = 2f;
        _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateAudioClip);
        _npcController.NavMeshAgent.SetDestination(NearestPointOnTargetFromPlayer());
        _npcController.FearReductionHasCooldown = true;

        // This while loop continues as long as the NPC's navigation path is still being calculated (pathPending) 
        // or the remaining distance to the target is greater than the stopping distance. 
        // This ensures the NPC continues moving until it has reached its destination.

        while(_npcController.NavMeshAgent.pathPending || _npcController.NavMeshAgent.remainingDistance > _npcController.NavMeshAgent.stoppingDistance)
        {
            _npcController.NavMeshAgent.SetDestination(NearestPointOnTargetFromPlayer());
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        _npcController.FearReductionHasCooldown = false;
        _npcController.CurrentState = _npcController.RoamState;
        _npcController.NpcAudioSource.PlayOneShot(_npcController.InvestigateEndAudioClip);
        _npcController.NavMeshAgent.SetDestination(_npcController.RoamTargetLocation.position);
        yield break;
    }

    /// <summary>
    /// Returns the nearest point on the target from the player.
    /// </summary>
    private Vector3 NearestPointOnTargetFromPlayer()
    {
        return _investigateTargetCollider.ClosestPoint(_npcController.transform.position);
    }
}
