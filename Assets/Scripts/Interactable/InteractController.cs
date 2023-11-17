using StarterAssets;
using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private PossessionController _possessionController;
    private VisionController _visionController;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _hoverMessage;

    private void Start()
    {
        _possessionController = GetComponent<PossessionController>();
        _visionController = GetComponent<VisionController>();
        _visionController.LookingAtChanged.AddListener(HandleDisplayingInteractPrompt);
    }

    public void Interact()
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

    private void HandleDisplayingInteractPrompt()
    {
        _hoverMessage.enabled = false;
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            if (objectInView.TryGetComponent(out Interactable interactable))
            {
                _hoverMessage.enabled = true;
                _hoverMessage.text = "Press [F] to " + interactable.HoverMessage;
                return;
            }
        }
    }
}
