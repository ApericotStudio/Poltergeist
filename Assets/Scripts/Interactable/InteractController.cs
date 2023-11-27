using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private PossessionController _possessionController;
    private VisionController _visionController;

    [Header("References")]
    [SerializeField] public TextMeshProUGUI HoverMessage;

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
                HandleDisplayingInteractPrompt();
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
        HoverMessage.enabled = false;
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            if (objectInView.TryGetComponent(out Interactable interactable) && !interactable.InteractDepleted)
            {
                HoverMessage.enabled = true;
                HoverMessage.text = "Press [F] to " + interactable.HoverMessage;
                return;
            }
        }
    }
}
