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
        PlayRandomScreamClip();
        _npcController.Agent.SetDestination(_npcController.CurrentRoamOrigin.position);
        yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 0.5f && !_npcController.Agent.pathPending);
        _npcController.CurrentState = _npcController.RoamState;
    }
    private void PlayRandomScreamClip()
    {
        _npcController.NpcAudioSource.PlayOneShot(_npcController.ScreamAudioClips[Random.Range(0, _npcController.ScreamAudioClips.Length)]);
    }
}