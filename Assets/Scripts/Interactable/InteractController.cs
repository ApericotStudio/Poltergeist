using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private PossessionController _possessionController;
    private VisionController _visionController;
    private bool isPossessed = false;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _hoverMessage;

    private void Start()
    {
        _possessionController = GetComponent<PossessionController>();
        _visionController = GetComponent<VisionController>();

        _visionController.LookingAtChanged.AddListener(HandleDisplayingInteractPrompt);
        _possessionController.CurrentPossessionChanged.AddListener(CheckPossession);
    }
    private void Update()
    {
        Debug.Log(isPossessed);
    }


    private void CheckPossession(GameObject currentPossession)
    {
        if (currentPossession != null)
        {
            isPossessed = true;
        }

        else
        {
            isPossessed = false;
        }
    }
    public void Interact()
    {
        if (isPossessed)
        {
            return;
        }

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
            if (objectInView.TryGetComponent(out Interactable interactable) && !interactable.InteractDepleted && !isPossessed)
            {
                _hoverMessage.enabled = true;
                _hoverMessage.text = "Press [E] to " + interactable.HoverMessage;
                return;
            }
        }
    }
}
