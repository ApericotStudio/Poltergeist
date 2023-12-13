using UnityEngine;
using UnityEngine.UI;

public class LevelCardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _selectButton;

    public delegate void LevelSelected(Level level);
    public event LevelSelected OnLevelSelected;

    private Level _level;

    public void Setup(Level level)
    {
        _level = level;
        _selectButton.onClick.AddListener(OnSelectButtonPressed);
    }

    private void OnSelectButtonPressed()
    {
        OnLevelSelected.Invoke(_level);
    }
}
