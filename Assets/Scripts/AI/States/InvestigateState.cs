using System.Collections;
using UnityEngine;

public class InvestigateState : IState
{
    private readonly AiController _aiController;

    private Collider _investigateTargetCollider;

    private IState _stateToReturnTo;

    public InvestigateState(AiController aiController, IState stateToReturnTo)
    {
        _aiController = aiController;
        _stateToReturnTo = stateToReturnTo;
    }

    public void Handle()
    {
        _investigateTargetCollider = _aiController.InvestigateTarget.GetComponent<Collider>();
        _aiController.StartCoroutine(InvestigateCoroutine());
    }

    /// <summary>
    /// Moves the AI to the location of the object that made a sound and then back to the roam location.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InvestigateCoroutine()
    {
        _aiController.Agent.speed = _aiController.InvestigatingSpeed;
        _aiController.Agent.stoppingDistance = 1f;
        _aiController.Agent.SetDestination(NearestPointOnTargetFromPlayer());

        // This while loop continues as long as the AI's navigation path is still being calculated (pathPending) 
        // or the remaining distance to the target is greater than the stopping distance. 
        // This ensures the NPC continues moving until it has reached its destination.

        while (_aiController.Agent.pathPending || _aiController.Agent.remainingDistance > _aiController.Agent.stoppingDistance)
        {
            if (_aiController.CurrentState is not InvestigateState)
            {
                yield break;
            }
            _aiController.Agent.SetDestination(NearestPointOnTargetFromPlayer());
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        if(IsInvestigating())
        {
            _aiController.CurrentState = _stateToReturnTo;
        }
    }

    /// <summary>
    /// Returns the nearest point on the target from the player.
    /// </summary>
    private Vector3 NearestPointOnTargetFromPlayer()
    {
        return _investigateTargetCollider.ClosestPoint(_aiController.transform.position);
    }

    private bool IsInvestigating()
    {
        return _aiController.CurrentState is InvestigateState;
    }
}
