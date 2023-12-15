using System.Collections;
using UnityEngine;

public class IdleState : IState
{
    private readonly VisitorController _npcController;

    public IdleState(VisitorController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(IdleCoroutine());
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        if (IsIdle())
        {
            _npcController.CurrentState = _npcController.RoamStateInstance;
        }
    }

    private bool IsIdle()
    {
        return _npcController.CurrentState is IdleState;
    }
}
