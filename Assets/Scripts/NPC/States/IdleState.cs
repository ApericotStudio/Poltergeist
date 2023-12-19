using System.Collections;
using UnityEngine;

public class IdleState : IState
{
    private readonly NpcController _npcController;

    private IState _stateToReturnTo;
    private float _timeIdle;

    public IdleState(NpcController _npcController)
    {
        this._npcController = _npcController;
        this._stateToReturnTo = null;
        this._timeIdle = 0;
    }
    public IdleState(NpcController _npcController, IState StateToReturnTo, float TimeIdle)
    {
        this._npcController = _npcController;
        this._stateToReturnTo = StateToReturnTo;
        this._timeIdle = TimeIdle;
    }

    public void Handle()
    {
        if (_stateToReturnTo == null)
        {
            _npcController.StartCoroutine(IdleCoroutine());
        } else
        {
            _npcController.StartCoroutine(IdleCoroutineWithTimer());
        }

        
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        if (IsIdle())
        {
            if (_npcController is VisitorController)
            {
                VisitorController _visitorController = _npcController as VisitorController;
                _visitorController.CurrentState = _visitorController.RoamStateInstance;
            }
        }
    }

    private IEnumerator IdleCoroutineWithTimer()
    {
        yield return new WaitForSeconds(_timeIdle);
        _npcController.CurrentState = _stateToReturnTo;
    }

    private bool IsIdle()
    {
        return _npcController.CurrentState is IdleState;
    }
}
