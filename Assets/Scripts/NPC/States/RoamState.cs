using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : IState
{
    private readonly VisitorController _visitorController;
    private Coroutine _periodicRoamCoroutine;

    public RoamState(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(RoamCoroutine());
        _periodicRoamCoroutine ??= _visitorController.StartCoroutine(PeriodicallySwitchRoomCoroutine());
    }

    public void StopStateCoroutines()
    {
        _visitorController.StopCoroutine(RoamCoroutine());
    }

    private IEnumerator RoamCoroutine()
    {
        _visitorController.Agent.stoppingDistance = 1f;
        _visitorController.Agent.speed = _visitorController.RoamingSpeed;

        while (IsRoaming())
        {
            Transform inspectTarget = _visitorController.CurrentRoom.GetRandomInspectableObject(_visitorController.InspectTarget);
            _visitorController.InspectTarget = inspectTarget;
            Vector3 newRoamLocation = GetClosestLocationToInspectTarget();
            _visitorController.Agent.SetDestination(newRoamLocation);
            yield return new WaitUntil(() => _visitorController.Agent.remainingDistance < 1f && !_visitorController.Agent.pathPending && IsRoaming());
            if(IsRoaming())
            {
                _visitorController.CurrentState = _visitorController.IdleStateInstance;
            }
        }
    }
    
    /// <summary>
    /// Periodically switches the room the visitor is roaming in.
    /// </summary>
    private IEnumerator PeriodicallySwitchRoomCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => _visitorController.Agent.remainingDistance < 0.5f && !_visitorController.Agent.pathPending);
            yield return new WaitForSeconds(_visitorController.TimeToSpendInRoom);
            _visitorController.SwitchRooms();
            yield return new WaitUntil(() => IsRoaming());
            _visitorController.Agent.SetDestination(GetClosestLocationToInspectTarget());
        }
    }

    /// <summary>
    /// Returns the closest location to the inspect target.
    /// </summary>
    public Vector3 GetClosestLocationToInspectTarget()
    {
        Vector3 closestLocation = _visitorController.InspectTarget.position; 
        NavMesh.SamplePosition(closestLocation, out NavMeshHit hit, _visitorController.RoamRadius, 1);
        return hit.position;
    }

    private bool IsRoaming()
    {
        return _visitorController.CurrentState is RoamState;
    }
}
