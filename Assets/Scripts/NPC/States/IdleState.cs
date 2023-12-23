using System.Collections;
using UnityEngine;

public class IdleState : IState
{
    private readonly NpcController _npcController;

    private IState _stateToReturnTo;
    private Animator _animator;
    private string _animationName;

    public IdleState(NpcController _npcController)
    {
        this._npcController = _npcController;
    }
    public IdleState(NpcController _npcController, IState StateToReturnTo, Animator _animator, string _animationName)
    {
        this._npcController = _npcController;
        this._stateToReturnTo = StateToReturnTo;
        this._animator = _animator;
        this._animationName = _animationName;
    }

    public void Handle()
    {
        if (_stateToReturnTo == null)
        {
            _npcController.StartCoroutine(IdleCoroutine());
        } else
        {
            _npcController.StartCoroutine(IdleCoroutineUntilAnimation());
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

    private IEnumerator IdleCoroutineUntilAnimation()
    {
        yield return new WaitWhile(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName(_animationName));
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animationName));
        _npcController.CurrentState = _stateToReturnTo;
    }

    private bool IsIdle()
    {
        return _npcController.CurrentState is IdleState;
    }
}
