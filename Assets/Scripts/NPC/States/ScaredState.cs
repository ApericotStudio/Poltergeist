using System.Collections;
using System.Linq;
using UnityEngine;

public class ScaredState : INpcState
{
    private readonly NpcController _npcController;

    public ScaredState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(ScareRoutine());
    }

    private IEnumerator ScareRoutine()
    {
        _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
        Transform transform = _npcController.RoamingTargets.FirstOrDefault();
        if (transform != null)
        {
            _npcController.RoamingTargets.Remove(transform);
            _npcController.NavMeshAgent.SetDestination(transform.position);
            while (_npcController.NavMeshAgent.pathPending)
            {
                yield return null;
            }
        }
        else
        {
            _npcController.CurrentState = new PanickedState(_npcController);
            yield break;
        }
        while (true)
        {
            if (_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                _npcController.RoamTargetLocation = transform;
                _npcController.CurrentState = new RoamState(_npcController);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
