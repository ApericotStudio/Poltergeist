using System.Collections;
using UnityEngine;

public class PhobiaState : IState
{
    private readonly VisitorController _visitorController;

    public PhobiaState(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(ScaredCoroutine());
    }

    public void StopStateCoroutines()
    {
        _visitorController.StopCoroutine(ScaredCoroutine());
    }

    private IEnumerator ScaredCoroutine()
    {
        _visitorController.Agent.speed = _visitorController.PanickedSpeed;
        _visitorController.SwitchRooms();
        _visitorController.Agent.stoppingDistance = 0f;
        _visitorController.Agent.SetDestination(_visitorController.CurrentRoom.transform.position);
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
