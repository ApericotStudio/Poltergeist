using System.Collections;
using UnityEngine;

public class IdleState : IState
{
    private readonly NpcController _npcController;

    private readonly IState _stateToReturnTo;
    private readonly Animator _animator;
    private readonly string _animationName;

    public IdleState(NpcController npcController)
    {
        _npcController = npcController;
    }
    public IdleState(NpcController npcController, IState stateToReturnTo, Animator animator, string animationName)
    {
        _npcController = npcController;
        _stateToReturnTo = stateToReturnTo;
        _animator = animator;
        _animationName = animationName;
    }

    public void Handle()
    {
        if (_stateToReturnTo == null)
        {
            _npcController.StartCoroutine(IdleCoroutine());
        } 
        else
        {
            _npcController.StartCoroutine(IdleCoroutineUntilAnimation());
        }
    }

    public void StopStateCoroutines()
    {
        _npcController.StopCoroutine(IdleCoroutine());
        _npcController.StopCoroutine(IdleCoroutineUntilAnimation());
    }

    private IEnumerator IdleCoroutine()
    {
        _npcController.LookAt(_npcController.InspectTarget);
        _npcController.StartCoroutine(_npcController.UpdateLookWeight(1f));
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        _npcController.StartCoroutine(_npcController.UpdateLookWeight(0f));
        yield return new WaitUntil(() => _npcController.LookWeight <= 0.1f);
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
