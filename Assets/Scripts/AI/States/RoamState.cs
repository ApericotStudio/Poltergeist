using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : IState
{
    private readonly NpcController _npcController;

    public RoamState(NpcController npcController)
    {
        _npcController = npcController;
    }

    public void Handle()
    {
        _npcController.StartCoroutine(RoamCoroutine());
        _npcController.StartCoroutine(PeriodicallySwitchRoomCoroutine());
    }

    private IEnumerator RoamCoroutine()
    {
        _npcController.Agent.stoppingDistance = 1f;
        _npcController.Agent.speed = _npcController.RoamingSpeed;

        while (IsRoaming())
        {
            Transform inspectTarget = _npcController.CurrentRoom.GetRandomInspectableObject(_npcController.CurrentInspectTarget);
            _npcController.CurrentInspectTarget = inspectTarget;
            Vector3 newRoamLocation = GetClosestLocationToInspectTarget();
            _npcController.Agent.SetDestination(newRoamLocation);
            yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 1f && !_npcController.Agent.pathPending && IsRoaming());
            _npcController.LookAt(inspectTarget);
            _npcController.CurrentState = _npcController.IdleStateInstance;
        }
    }
    
    /// <summary>
    /// Periodically switches the room the NPC is roaming in.
    /// </summary>
    private IEnumerator PeriodicallySwitchRoomCoroutine()
    {
        while (IsRoaming())
        {
            _npcController.Agent.SetDestination(GetClosestLocationToInspectTarget());
            yield return new WaitUntil(() => _npcController.Agent.remainingDistance < 0.5f && !_npcController.Agent.pathPending);
            yield return new WaitForSeconds(_npcController.RoomTimeSpent);
            _npcController.SwitchRooms();
        }
    }

    /// <summary>
    /// Returns the closest location to the inspect target.
    /// </summary>
    public Vector3 GetClosestLocationToInspectTarget()
    {
        Vector3 closestLocation = _npcController.CurrentInspectTarget.position; 
        NavMesh.SamplePosition(closestLocation, out NavMeshHit hit, _npcController.RoamRadius, 1);
        return hit.position;
    }

    private bool IsRoaming()
    {
        return _npcController.CurrentState is RoamState;
    }
}
