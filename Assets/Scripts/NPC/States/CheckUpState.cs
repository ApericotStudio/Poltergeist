using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The state in which the realtor checks up on the visitors.
/// </summary>
public class CheckUpState : IState
{
    private readonly RealtorController _realtorController;
    
    public CheckUpState(RealtorController realtorController)
    {
        _realtorController = realtorController;
    }

    public void Handle()
    {
        _realtorController.StartCoroutine(CheckUpCoroutine());
        _realtorController.StartCoroutine(PeriodicallySetCheckUpOriginCoroutine());
    }

    private IEnumerator CheckUpCoroutine()
    {
        _realtorController.Agent.stoppingDistance = 0f;
        _realtorController.Agent.speed = _realtorController.RoamingSpeed;

        while (IsCheckingUp())
        {
            if (_realtorController.Agent.remainingDistance < 0.5f)
            {
                Vector3 newRoamLocation = GetRoamLocation();
                _realtorController.Agent.SetDestination(newRoamLocation);
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    /// <summary>
    /// Sets the check up origin location every x seconds.
    /// </summary>
    private IEnumerator PeriodicallySetCheckUpOriginCoroutine()
    {
        while (IsCheckingUp())
        {
            _realtorController.Agent.SetDestination(GetRoamLocation());
            yield return new WaitUntil(() => _realtorController.Agent.remainingDistance < 0.5f && !_realtorController.Agent.pathPending);
            yield return new WaitForSeconds(_realtorController.CheckUpTimeSpent);
            _realtorController.SetNextCheckupOrigin();
        }
    }

    private bool IsCheckingUp()
    {
        return _realtorController.CurrentState is CheckUpState;
    }

    /// <summary>
    /// Returns a random location within the roam radius around the check up origin.
    /// </summary>
    private Vector3 GetRoamLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _realtorController.CheckUpRadius;
        randomDirection += _realtorController.CurrentCheckUpOrigin.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _realtorController.CheckUpRadius, 1);
        return hit.position;
    }
}

