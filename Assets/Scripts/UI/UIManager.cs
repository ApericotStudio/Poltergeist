using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Menu _menu;
    [SerializeField]
    private TMP_Text _gameOverText;

    public void OpenLoseScreen()
    {
        _gameOverText.text = "You lost!";
        _menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void OpenWinScreen()
    {
       _gameOverText.text = "You won!";
       _menu.gameObject.SetActive(true); 
       Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        _menu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
