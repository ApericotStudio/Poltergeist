using UnityEngine;

public class InteractController : MonoBehaviour
{
    private VisionController _visionController;
    private PossessionController _possessionController;

    private void Start()
    {
        _visionController = GetComponent<VisionController>();
        _possessionController = GetComponent<PossessionController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    private void Interact()
    {
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            if (objectInView.TryGetComponent(out Interactable interactable))
            {
                interactable.Use();
                return;
            }
        }
        if (_possessionController.CurrentPossession == null)
        {
            return;
        }
        if (!_possessionController.CurrentPossession.TryGetComponent(out Interactable possessedInteractable))
        {
            return;
        }
        possessedInteractable.Use();
    }
}
