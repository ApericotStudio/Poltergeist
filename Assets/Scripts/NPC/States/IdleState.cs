using System.Collections;
using UnityEngine;

public class IdleState : IState
{
    private readonly VisitorController _visitorController;

    public IdleState(VisitorController _visitorController)
    {
        this._visitorController = _visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(IdleCoroutine());
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        if (IsIdle())
        {
            _visitorController.CurrentState = _visitorController.RoamStateInstance;
        }
    }

    private bool IsIdle()
    {
        return _visitorController.CurrentState is IdleState;
    }
}
