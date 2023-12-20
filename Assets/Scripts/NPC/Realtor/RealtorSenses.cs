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
    [Tooltip("Cooldown of soothing."), Range(0f, 1f), SerializeField]
    private float _sootheCooldown = 15f;
    private float _sootheCooldownTimer = 5;

    [HideInInspector]
    public List<VisitorController> SoothedVisitors = new();

    private RealtorController _realtorController;

    protected override void Awake()
    {
        base.Awake();
        _realtorController = GetComponent<RealtorController>();
        StartCoroutine(DecreaseNpcFear());
    }

    private void Update()
    {
        if (_sootheCooldownTimer > 0f) { _sootheCooldownTimer -= Time.deltaTime; }
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
                    DetectedVisitors[i].OnFearValueChange.AddListener(Soothe);
                    SoothedVisitors.Add(DetectedVisitors[i]);
                }
            }
            else
            {
                if (SoothedVisitors.Contains(DetectedVisitors[i]))
                {
                    DetectedVisitors[i].SeenByRealtor = false;
                    DetectedVisitors[i].OnFearValueChange.RemoveListener(Soothe);
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
            visitor.OnFearValueChange.RemoveListener(Soothe);
        }
        SoothedVisitors.Clear();
    }

    private void Soothe(float fear)
    {
        if (fear > 20f && _sootheCooldownTimer <= 0)
        {
            _realtorController.Agent.isStopped = true;
            _realtorController.Animator.SetTrigger("Soothe");
            _sootheCooldownTimer = _sootheCooldown;
            StartCoroutine(WaitAnimation());
        }
    }

    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(() => _realtorController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Soothe"));
        _realtorController.Agent.isStopped = false;
    }

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
