using System.Collections;
using UnityEngine;

public class ScareCamController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _scareCam;
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
        _scareCam.enabled = false;
        _scareCamImage.SetActive(false);
    }

    private void SetFollowTarget(Transform target)
    {
        _scareCam.transform.SetParent(target);
        _scareCam.transform.localPosition = _cameraOffset;
        _scareCam.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        _scareCam.enabled = true;
        _scareCamImage.SetActive(true);
    }
}
