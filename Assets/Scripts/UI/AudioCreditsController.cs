using UnityEngine;
using UnityEngine.UI;

public class AudioCreditsController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button _backButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        Destroy(gameObject);
    }
}
