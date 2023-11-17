using System.Collections;
using UnityEngine;

public class PanickedState : INpcState
{
    private readonly NpcController _npcController;

    public PanickedState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(PanickedCoroutine());
    }

    IEnumerator PanickedCoroutine()
    {
        _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
        _npcController.NavMeshAgent.stoppingDistance = 0f;
        _npcController.NavMeshAgent.SetDestination(_npcController.FrightenedTargetLocation.position);
        PlayRandomScreamClip();
        while (true)
        {
            if (_npcController.NavMeshAgent.pathPending && _npcController.NavMeshAgent.velocity.magnitude > 0 && _npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                _npcController.RanAway = true;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void PlayRandomScreamClip()
    {
        _npcController.NpcAudioSource.PlayOneShot(_npcController.ScreamAudioClips[Random.Range(0, _npcController.ScreamAudioClips.Length)]);
    }
}

