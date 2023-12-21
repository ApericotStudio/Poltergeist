using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
