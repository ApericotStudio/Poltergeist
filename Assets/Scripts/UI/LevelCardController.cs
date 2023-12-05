using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Image _thumbnail;
    [SerializeField] private Button _selectButton;

    private string sceneName;

    public void Setup(Level level)
    {
        _title.text = level.Title;
        _thumbnail.sprite = level.Thumbnail;
        sceneName = level.SceneName;
        _selectButton.onClick.AddListener(OnSelectButtonPressed);
    }

    private void OnSelectButtonPressed()
    {
        SceneManager.LoadScene(sceneName);
    }
}
