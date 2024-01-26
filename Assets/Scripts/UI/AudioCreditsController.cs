using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioCreditsController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _backButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_backButton.gameObject);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
