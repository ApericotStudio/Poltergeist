using System.Collections;
using UnityEngine;

public class PanickedState : INpcState
{
    private readonly NpcController _npcController;
    private readonly AudioSource _npcAudioSource;
    private readonly AudioClip[] _screamClips;

    public PanickedState(NpcController npcController)
    {
        _npcController = npcController;
        _npcAudioSource = _npcController.NpcAudioSource;
        _screamClips = _npcController.ScreamAudioClips;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(RunAway());
    }

    IEnumerator RunAway()
    {
        _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
        _npcController.NavMeshAgent.SetDestination(_npcController.FrightenedTargetLocation.position);
        PlayRandomScreamClip();
        while (true)
        {
            if (_npcController.NavMeshAgent.velocity.magnitude > 0 && _npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                _npcController.RanAway = true;
                _npcController.GameEventManager.OnGameEvent.Invoke(GameEvents.PlayerWon);
                _npcController.StopCoroutine(RunAway());
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void PlayRandomScreamClip()
    {
        _npcAudioSource.PlayOneShot(_screamClips[Random.Range(0, _screamClips.Length)]);
    }
}

