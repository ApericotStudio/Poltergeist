using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SootheState : IState
{
    private readonly NpcController _npcController;

    public VisitorController Visitor;

    private Animator _animator;
    private NavMeshAgent _agent;

    [Tooltip("Cooldown of soothing."), Range(0f, 30f), SerializeField]
    private float _sootheCooldown = 0f;
    private float _sootheCooldownTimer = 0;

    public SootheState(NpcController npcController)
    {
        _npcController = npcController;
        _animator = _npcController.Animator;
        _agent = _npcController.Agent;

    }

    public void Handle()
    {
        SootheCoroutine();
    }

    public void StopStateCoroutines()
    {
        
    }

    /// <summary>
    /// Moves the npc to the location of the object that made a sound and then back to the roam location.
    /// </summary>
    /// <returns></returns>
    public void SootheCoroutine()
    {
        _sootheCooldownTimer = _sootheCooldown;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger("Soothe");
        _npcController.LookAt(Visitor.gameObject.GetComponent<VisitorSenses>().HeadTransform);
        _npcController.StartCoroutine(WaitForSoothe(Visitor));
    }

    private IEnumerator WaitForSoothe(VisitorController visitor)
    {
        //wait until animation is Soothe animation.
        yield return new WaitWhile(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName("Soothe"));
        //rotate to visitor while you are in the soothe animation.
        while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Soothe"))
        {
            Vector3 rotationDir = visitor.transform.position - _npcController.transform.position;
            rotationDir.y = 0;
            Quaternion rotationQuat = Quaternion.LookRotation(rotationDir);
            _npcController.transform.rotation = Quaternion.Lerp(_npcController.transform.rotation, rotationQuat, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _npcController.LookAt(_npcController.InspectTarget);
        _agent.isStopped = false;
        _npcController.CurrentState = _npcController.InvestigateStateInstance;
    }
}
