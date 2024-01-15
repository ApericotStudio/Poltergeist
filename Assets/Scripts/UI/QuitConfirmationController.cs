using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitConfirmationController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    [Header("Other")]
    [SerializeField] private string _mainMenuSceneName;

    private void Awake()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_cancelButton.gameObject);
    }

    private void SetupButtons()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonPressed);
        _cancelButton.onClick.AddListener(OnCancelButtonPressed);
    }

    private void OnConfirmButtonPressed()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    private void OnCancelButtonPressed()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }
}
