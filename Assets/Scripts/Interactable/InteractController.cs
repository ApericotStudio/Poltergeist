using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private PossessionController _possessionController;
    private VisionController _visionController;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _hoverMessage;

    public delegate void Interaction(int index);
    public event Interaction hasInteracted;

    private void Start()
    {
        _possessionController = GetComponent<PossessionController>();
        _visionController = GetComponent<VisionController>();

        _visionController.LookingAtChanged.AddListener(HandleDisplayingInteractPrompt);

    }

    public void Interact()
    {
        hasInteracted?.Invoke(3);
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
        _hoverMessage.enabled = false;
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            if (objectInView.TryGetComponent(out Interactable interactable) && !interactable.InteractDepleted)
            {
                _hoverMessage.enabled = true;
                _hoverMessage.text = "Press [E] to " + interactable.HoverMessage;
                return;
            }
        }
    }
}
