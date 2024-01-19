using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The state in which the realtor checks up on the visitors.
/// </summary>
public class CheckUpState : IState
{
    private readonly RealtorController _realtorController;
    private Coroutine _periodicCheckUpCoroutine;
    
    public CheckUpState(RealtorController realtorController)
    {
        _realtorController = realtorController;
    }

    public void Handle()
    {
        _periodicCheckUpCoroutine ??= _realtorController.StartCoroutine(PeriodicallySetCheckUpOriginCoroutine());
        _realtorController.StartCoroutine(CheckUpCoroutine());
    }

    public void StopStateCoroutines()
    {
        _realtorController.StopCoroutine(CheckUpCoroutine());
    }

    private IEnumerator CheckUpCoroutine()
    {
        if(_realtorController.LookWeight > 0f)
        {
            _realtorController.StartCoroutine(_realtorController.UpdateLookWeight(0f));          
        }
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
        
        // Keep finding a point that is not behind a wall
        int raycasts = 0;
        int maxRaycasts = 3;
        while (!Physics.Raycast(hit.position, _realtorController.CurrentCheckUpOrigin.position - hit.position,  _realtorController.CheckUpRadius, 1) && raycasts < maxRaycasts)
        {
            NavMesh.SamplePosition(randomDirection, out hit, _realtorController.CheckUpRadius, 1);
            raycasts++;
        }

        return hit.position;
    }
}

