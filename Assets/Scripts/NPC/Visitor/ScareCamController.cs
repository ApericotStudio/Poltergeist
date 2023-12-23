using System.Collections;
using UnityEngine;

public class ScareCamController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _scareCamImage;
    [SerializeField] private Transform _visitorCollection;

    [Header("Adjustable Variables")]
    [SerializeField] private Vector3 _cameraOffset = Vector3.zero;
    [SerializeField] private float _followDuration = 2f;

    private void Awake()
    {
        foreach (ScareCamTrigger visitor in _visitorCollection.GetComponentsInChildren<ScareCamTrigger>())
        {
            visitor.OnScareCamTriggered += OnScareCamTriggered;
        }
    }

    private void OnScareCamTriggered(Transform head)
    {
        StopAllCoroutines();
        SetFollowTarget(head);
        StartCoroutine(ClearFollowTarget());
    }

    private IEnumerator ClearFollowTarget()
    {
        yield return new WaitForSeconds(_followDuration);
        _scareCamImage.SetActive(false);
    }

    private void SetFollowTarget(Transform target)
    {
        transform.SetParent(target);
        transform.localPosition = _cameraOffset;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        _scareCamImage.SetActive(true);
    }
}
