using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMappingController : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _mouseKeyboardButton;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_mouseKeyboardButton.gameObject);
    }
    
    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }
}
