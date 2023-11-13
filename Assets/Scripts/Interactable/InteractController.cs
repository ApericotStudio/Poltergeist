using UnityEngine;

public class InteractController : MonoBehaviour
{
    private VisionController _visionController;

    private void Start()
    {
        _visionController = GetComponent<VisionController>();
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
        if (objectInView == null)
        {
            return;
        }
        if (!objectInView.TryGetComponent(out Interactable interactable))
        {
            return;
        }
        interactable.Use();
    }
}
