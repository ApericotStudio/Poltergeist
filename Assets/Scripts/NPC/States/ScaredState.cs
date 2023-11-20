public class ScaredState : INpcState
{
    private readonly NpcController _npcController;

    public ScaredState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        if (!_npcController.RanAway)
        {
            _npcController.NavMeshAgent.speed = _npcController.FrightenedSpeed;
            _npcController.NavMeshAgent.SetDestination(_npcController.FrightenedTargetLocation.position);

            if(_npcController.NavMeshAgent.remainingDistance < 0.5f)
            {
                _npcController.CurrentRoamTargetLocation = _npcController.FrightenedTargetLocation;
                _npcController.CurrentState = new RoamState(_npcController);
            }
        }
    }
}
