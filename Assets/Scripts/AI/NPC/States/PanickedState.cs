using System.Collections;
using UnityEngine;

public class PanickedState : IState
{
    private readonly NpcController _npcController;

    public PanickedState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(PanickedCoroutine());
    }

    IEnumerator PanickedCoroutine()
    {
        _npcController.Agent.speed = _npcController.FrightenedSpeed;
        _npcController.Agent.stoppingDistance = 0f;
        _npcController.Agent.SetDestination(_npcController.FrightenedTargetLocation.position);
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

