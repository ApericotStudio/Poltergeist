using UnityEngine;

public class PolterSenseTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PolterSenseController _polterSenseController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Outline outline))
        {
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
