using System.Collections;
using UnityEngine;

public class PanickedState : IState
{
    private readonly VisitorController _visitorController;

    public PanickedState(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(PanickedCoroutine());
    }

    private IEnumerator PanickedCoroutine()
    {
        _visitorController.Agent.speed = _visitorController.PanickedSpeed;
        _visitorController.Agent.stoppingDistance = 0f;
        _visitorController.Agent.SetDestination(_visitorController.PanickedTargetLocation.position);
        while (true)
        {
            if (_visitorController.Agent.pathPending && _visitorController.Agent.velocity.magnitude > 0 && _visitorController.Agent.remainingDistance < 0.5f)
            {
                _visitorController.RanAway = true;
                _visitorController.Agent.isStopped = true;
                _visitorController.Agent.enabled = false;
                _visitorController.gameObject.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

