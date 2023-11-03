using UnityEngine;

public class PanickedState : INpcState
{
    private readonly NpcController _npcController;
    private readonly AudioSource _npcAudioSource;
    private readonly AudioClip[] _screamClips;
    private bool hasScreamed = false;

    public PanickedState(NpcController npcController)
    {
        _npcController = npcController;
        _npcAudioSource = _npcController.NpcAudioSource;
        _screamClips = _npcController.ScreamAudioClips;
    }

    public void Execute()
    {
        if (_npcController.RanAway)
        {
           return;
        }
        _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
        _npcController.NavMeshAgent.SetDestination(_npcController.FrightenedTargetLocation.position);

        if (!hasScreamed)
        {
            Scream();
        }


        if(_npcController.NavMeshAgent.velocity.magnitude > 0 &&_npcController.NavMeshAgent.remainingDistance < 0.5f)
        {
            _npcController.RanAway = true;
            _npcController.GameEventManager.OnGameEvent.Invoke(GameEvents.PlayerWon);
        }
    }

    private void Scream()
    {
        _npcAudioSource.PlayOneShot(_screamClips[Random.Range(0, _screamClips.Length)]);
        hasScreamed = true;
    }
}

