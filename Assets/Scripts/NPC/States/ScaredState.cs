using System.Collections;
using UnityEngine;

public class ScaredState : IState
{
    private readonly VisitorController _visitorController;

    public ScaredState(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(ScaredCoroutine());
    }

    private IEnumerator ScaredCoroutine()
    {
        _visitorController.Agent.speed = _visitorController.PanickedSpeed;
        _visitorController.SwitchRooms();
        _visitorController.Agent.stoppingDistance = 0f;
        _visitorController.Agent.SetDestination(_visitorController.CurrentRoom.GetRandomInspectableObject(null).position);
        yield return new WaitUntil(() => _visitorController.Agent.remainingDistance < 0.5f && !_visitorController.Agent.pathPending);
        if(IsScared())
        {
            _visitorController.CurrentState = _visitorController.RoamStateInstance;
        }
    }

    private bool IsScared()
    {
        return _visitorController.CurrentState is ScaredState;
    }
}
