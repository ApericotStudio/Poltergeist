using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrightenedState : INpcState
{
    private readonly NpcController _npcController;

    public FrightenedState(NpcController npcController)
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
            }
        }
    }
}

