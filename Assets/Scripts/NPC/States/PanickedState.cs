public class PanickedState : INpcState
{
    private readonly NpcController _npcController;

    public PanickedState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Execute()
    {
        if (!_npcController.RanAway)
        {
            _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
            _npcController.NavMeshAgent.SetDestination(_npcController.FrightenedTargetLocation.position);

            if(_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                _npcController.RanAway = true;
                _npcController.GameEventManager.Won = true;
                _npcController.GameEventManager.OnGameEvent.Invoke(GameEvents.GameOver);
            }
        }
    }
}

