using System.Collections;
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
        _npcController.StartCoroutine(ScaredCoroutine());
    }

    private IEnumerator ScaredCoroutine()
    {
        _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
        _npcController.SetRoamOrigin();
        _npcController.NavMeshAgent.stoppingDistance = 0f;
        _npcController.NpcAudioSource.PlayOneShot(_npcController.SmallScreamAudioClips.GetRandom());
        _npcController.NavMeshAgent.SetDestination(_npcController.CurrentRoamOrigin.position);
        yield return new WaitUntil(() => _npcController.NavMeshAgent.remainingDistance < 0.5f && !_npcController.NavMeshAgent.pathPending);
        _npcController.CurrentState = _npcController.RoamState;
    }
}
