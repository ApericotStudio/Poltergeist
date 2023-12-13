using System.Collections;
using UnityEngine;

public class ScaredState : IState
{
    private readonly NpcController _npcController;

    public ScaredState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(ScaredCoroutine());
    }

    private IEnumerator ScaredCoroutine()
    {
        _npcController.Agent.speed = _npcController.FrightenedSpeed;
        _npcController.SetRoamOrigin();
        _npcController.Agent.stoppingDistance = 0f;
        _npcController.Agent.SetDestination(_npcController.CurrentRoamOrigin.position);
        yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 0.5f && !_npcController.Agent.pathPending);
        if(IsScared())
        {
            _npcController.CurrentState = _npcController.RoamStateInstance;
        }
    }

    private bool IsScared()
    {
        return _npcController.CurrentState is ScaredState;
    }
}
