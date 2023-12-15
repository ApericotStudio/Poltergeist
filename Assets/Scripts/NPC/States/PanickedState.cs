using System.Collections;
using UnityEngine;

public class PanickedState : IState
{
    private readonly VisitorController _npcController;

    public PanickedState(VisitorController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(PanickedCoroutine());
    }

    private IEnumerator PanickedCoroutine()
    {
        _npcController.Agent.speed = _npcController.PanickedSpeed;
        _npcController.Agent.stoppingDistance = 0f;
        _npcController.Agent.SetDestination(_npcController.PanickedTargetLocation.position);
        while (true)
        {
            if (_npcController.Agent.pathPending && _npcController.Agent.velocity.magnitude > 0 && _npcController.Agent.remainingDistance < 0.5f)
            {
                _npcController.RanAway = true;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

