using System.Collections;
using UnityEngine;

public class InvestigateState : IState
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
    private IEnumerator InvestigateCoroutine()
    {
        _npcController.Agent.speed = _npcController.InvestigatingSpeed;
        _npcController.Agent.stoppingDistance = 2f;
        _npcController.Agent.SetDestination(NearestPointOnTargetFromPlayer());
        _npcController.FearReductionHasCooldown = true;

        // This while loop continues as long as the NPC's navigation path is still being calculated (pathPending) 
        // or the remaining distance to the target is greater than the stopping distance. 
        // This ensures the NPC continues moving until it has reached its destination.

        while (_npcController.Agent.pathPending || _npcController.Agent.remainingDistance > _npcController.Agent.stoppingDistance)
        {
            if (_npcController.CurrentState is not InvestigateState)
            {
                yield break;
            }
            _npcController.Agent.SetDestination(NearestPointOnTargetFromPlayer());
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        _npcController.FearReductionHasCooldown = false;
        _npcController.Agent.SetDestination(_npcController.CurrentRoamOrigin.position);

        if(IsInvestigating())
        {
            _npcController.CurrentState = _npcController.RoamState;
        }
    }

    /// <summary>
    /// Returns the nearest point on the target from the player.
    /// </summary>
    private Vector3 NearestPointOnTargetFromPlayer()
    {
        return _investigateTargetCollider.ClosestPoint(_npcController.transform.position);
    }

    private bool IsInvestigating()
    {
        return _npcController.CurrentState is InvestigateState;
    }
}
