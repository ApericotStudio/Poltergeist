using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoamState : IState
{
    private readonly VisitorController _visitorController;
    
    public RandomRoamState(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    public void Handle()
    {
        _visitorController.StartCoroutine(RoamCoroutine());
        _visitorController.StartCoroutine(PeriodicallySetRoamOriginCoroutine());
    }

    public void StopStateCoroutines()
    {
        _visitorController.StopCoroutine(RoamCoroutine());
        _visitorController.StopCoroutine(PeriodicallySetRoamOriginCoroutine());
    }

    private IEnumerator RoamCoroutine()
    {
        _visitorController.Agent.stoppingDistance = 0f;
        _visitorController.Agent.speed = _visitorController.RoamingSpeed;

        while (IsRoaming())
        {
            if (_visitorController.Agent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
                _visitorController.Agent.SetDestination(newRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
    
    /// <summary>
    /// Sets the roam origin location every x seconds.
    /// </summary>
    private IEnumerator PeriodicallySetRoamOriginCoroutine()
    {
        while (IsRoaming())
        {
            _visitorController.Agent.SetDestination(GetRoamLocation());
            yield return new WaitUntil(() => _visitorController.Agent.remainingDistance < 0.5f && !_visitorController.Agent.pathPending);
            yield return new WaitForSeconds(_visitorController.TimeToSpendInRoom);
            _visitorController.SwitchRooms();
        }
    }

    /// <summary>
    /// Returns a random location within the roam radius around the roam origin.
    /// </summary>
    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _visitorController.RoamRadius;
        randomDirection += _visitorController.CurrentRoom.transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _visitorController.RoamRadius, 1);
        return hit.position;
    }

    private bool IsRoaming()
    {
        return _visitorController.CurrentState is RandomRoamState;
    }
}
