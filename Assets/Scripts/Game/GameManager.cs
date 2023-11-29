using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnEndGame = new UnityEvent();

    public void EndGame()
    {
        OnEndGame.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
    }
}
