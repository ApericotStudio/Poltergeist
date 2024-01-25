using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    private PossessionController _possessionController;
    private VisionController _visionController;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _hoverMessage;

    public delegate void Interaction(int index);
    public event Interaction HasInteracted;
    [Header("Prompts")]
    [SerializeField]
    private GameObject _mnkPrompt;
    [SerializeField]
    private GameObject _controllerPrompt;
    private PlayerInput _playerInput;

    private void Awake() 
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update() 
    {
        if(_hoverMessage.enabled) {
            if (_playerInput.currentControlScheme == "Gamepad") {
                _mnkPrompt.SetActive(false);
                _controllerPrompt.SetActive(true);
            } 
            else 
            {
                _mnkPrompt.SetActive(true);
                _controllerPrompt.SetActive(false);
            } 
        }
        else 
        {
            _mnkPrompt.SetActive(false);
            _controllerPrompt.SetActive(false);
        }
    }
    
    private void Start()
    {
        _possessionController = GetComponent<PossessionController>();
        _visionController = GetComponent<VisionController>();

        _visionController.LookingAtChanged.AddListener(HandleDisplayingInteractPrompt);
    }

    public void Interact()
    {
        if (_possessionController.CurrentPossession != null)
        {
            return;
        }

        HasInteracted?.Invoke(3);
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
    }

    private void HandleDisplayingInteractPrompt()
    {
        _hoverMessage.enabled = false;
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView != null)
        {
            if (objectInView.TryGetComponent(out Interactable interactable) && !interactable.InteractDepleted && _possessionController.CurrentPossession == null)
            {
                _hoverMessage.enabled = true;
                _hoverMessage.text = _hoverMessage.text.Replace("...", interactable.HoverMessage);
                return;
            }
        }
    }
}
