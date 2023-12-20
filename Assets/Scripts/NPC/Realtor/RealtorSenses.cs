using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtorSenses : BaseSenses
{
    [Header("Fear Reduction Settings")]
    [Tooltip("The distance that the realtor can reduce visitor fear."), Range(0, 50)]
    public float FearReductionRange = 10f;
    [Tooltip("The value that will be subtracted from the fear value of visitors close to the realtor."), Range(0f, 1f), SerializeField]
    private float _reductionValue = 0.1f;
    [Tooltip("The speed at which the fear value will be reduced."), Range(0f, 1f), SerializeField]
    private float _reductionSpeed = 0.05f;

    [HideInInspector]
    public List<VisitorController> SoothedVisitors = new();

    private RealtorController _realtorController;

    protected override void Awake()
    {
        _realtorController = GetComponent<RealtorController>();
        base.Awake();
        StartCoroutine(DecreaseNpcFear());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Visitors:");
            foreach (VisitorController visitor in SoothedVisitors)
            {
                Debug.Log(visitor.gameObject.name);
            }
        }
    }

    protected override void HandleTargets(Collider[] targetsInDetectionRadius)
    {
        base.HandleTargets(targetsInDetectionRadius);
        for (int i = 0; i < DetectedVisitors.Count; i++)
        {
            float distanceToVisitor = Vector3.Distance(transform.position, DetectedVisitors[i].transform.position);
            if (distanceToVisitor <= FearReductionRange)
            {
                if (!SoothedVisitors.Contains(DetectedVisitors[i]))
                {
                    DetectedVisitors[i].SeenByRealtor = true;
                    DetectedVisitors[i].OnFearValueChange.AddListener(_realtorController.Soothe);
                    SoothedVisitors.Add(DetectedVisitors[i]);
                }
            }
            else
            {
                if (SoothedVisitors.Contains(DetectedVisitors[i]))
                {
                    DetectedVisitors[i].SeenByRealtor = false;
                    DetectedVisitors[i].OnFearValueChange.RemoveListener(_realtorController.Soothe);
                    SoothedVisitors.Remove(DetectedVisitors[i]);
                }
            }  
        }
    }
    
    public override void OnNotify(ObservableObject observableObject)
    {
        if (!DetectedObjects.TryGetValue(observableObject, out _))
            return;

        switch (observableObject.State)
        {
            case ObjectState.Interacted:
                _realtorController.InspectTarget = observableObject.transform;
                _realtorController.Investigate();
                break;
            case ObjectState.Hit:
                if (observableObject.Type == ObjectType.Small)
                {
                    _realtorController.InspectTarget = observableObject.transform;
                    _realtorController.Investigate();
                }
                break;
            default:
                return;
            }
    }
    
    protected override void ClearDetectedTargets()
    {
        base.ClearDetectedTargets();
        foreach(VisitorController visitor in SoothedVisitors)
        {
            visitor.SeenByRealtor = false;
            visitor.OnFearValueChange.RemoveListener(_realtorController.Soothe);
        }
        SoothedVisitors.Clear();
    }

    //private void Soothe(float fear, VisitorController visitor)
    //{
    //    if (fear > 1f && _sootheCooldownTimer <= 0 && _realtorController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Walk Run Blend"))
    //    {
    //        Debug.Log("Soothe");
    //        _realtorController.LookAt(visitor.gameObject.transform);
    //        _realtorController.Agent.isStopped = true;
    //        _realtorController.Agent.velocity = Vector3.zero;
    //        _realtorController.Animator.SetTrigger("Soothe");
    //        _sootheCooldownTimer = _sootheCooldown;
    //        StartCoroutine(WaitForSoothe(visitor));
    //    }
    //}

    //private IEnumerator WaitForSoothe(VisitorController visitor)
    //{
    //    //wait until animation is Soothe animation.
    //    yield return new WaitWhile(() => !_realtorController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Soothe"));
    //    //rotate to visitor while you are in the soothe animation.
    //    while (_realtorController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Soothe"))
    //    {
    //        Vector3 rotationDir = visitor.transform.position - this.transform.position;
    //        rotationDir.y = 0;
    //        Quaternion rotationQuat = Quaternion.LookRotation(rotationDir);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, rotationQuat, Time.deltaTime);
    //        yield return new WaitForEndOfFrame();
    //    }
    //    _realtorController.Agent.isStopped = false;
    //    Debug.Log("Resume");
    //}

    private IEnumerator DecreaseNpcFear()
    {
        while (true)
        {
            if (DetectedVisitors.Count == 0)
            {
                yield return new WaitForSeconds(_reductionSpeed);
                continue;
            }
            foreach (VisitorController npc in DetectedVisitors)
            {
                npc.FearValue -= _reductionValue;
            }
            yield return new WaitForSeconds(_reductionSpeed);
        }
    }
}
