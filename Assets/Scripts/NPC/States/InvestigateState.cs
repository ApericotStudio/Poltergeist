using System.Collections;
using UnityEngine;

public class InvestigateState : IState
{
    private readonly NpcController _npcController;

    private Collider _investigateTargetCollider;

    private IState _stateToReturnTo;

    private float _investigateTime;

    public InvestigateState(NpcController npcController, IState stateToReturnTo)
    {
        _npcController = npcController;
        _stateToReturnTo = stateToReturnTo;

    }

    public void Handle()
    {
        _investigateTime = _npcController.InvestigateTime;
        _investigateTargetCollider = _npcController.InspectTarget.GetComponent<Collider>();
        _npcController.StartCoroutine(InvestigateCoroutine());
    }

    /// <summary>
    /// Moves the npc to the location of the object that made a sound and then back to the roam location.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InvestigateCoroutine()
    {
        _npcController.Agent.speed = _npcController.InvestigatingSpeed;
        _npcController.Agent.stoppingDistance = 1f;
        _npcController.Agent.SetDestination(NearestPointOnTargetFromPlayer());

        // This while loop continues as long as the npc's navigation path is still being calculated (pathPending) 
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

        yield return new WaitForSeconds(_timeLooking);

        if(IsInvestigating())
        {
            _npcController.CurrentState = _stateToReturnTo;
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
