using UnityEngine;
using UnityEngine.UI;

public class ButtonMappingController : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
