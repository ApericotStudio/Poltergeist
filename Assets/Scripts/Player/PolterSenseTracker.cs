using UnityEngine;

public class PolterSenseTracker : MonoBehaviour
{
    private SphereCollider _collider;

    [Header("References")]
    [SerializeField] private PolterSenseController _polterSenseController;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Outline outline))
        {
            print("outline found");
            _polterSenseController.AddOutline(outline);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Outline outline))
        {
            _polterSenseController.RemoveOutline(outline);
        }
    }
}
