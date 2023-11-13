using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)] private float _interactRange;
    private Camera _playerCamera;

    private void Start()
    {
        SetCamera(Camera.main);
    }

    private void SetCamera(Camera camera)
    {
        if (camera == null)
        {
            throw new System.Exception("Camera reference may not be null.");
        }
        _playerCamera = camera;
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
        Interactable interactable = LookForInteractableObject();
        if (interactable == null)
        {
            return;
        }
        interactable.Use();
    }

    /// <summary>
    /// Checks if the camera is looking at an interactable object
    /// </summary>
    /// <returns>Found interactable object or null</returns>
    private Interactable LookForInteractableObject()
    {
        if (_playerCamera == null)
        {
            throw new System.Exception("Missing camera reference.");
        }
        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _interactRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                return interactable;
            }
        }
        return null;
    }
}
