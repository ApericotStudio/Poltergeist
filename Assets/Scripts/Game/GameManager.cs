using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnEndGame = new UnityEvent();

    [Header("Adjustable variables")]
    [SerializeField] private float _timeRemaining;

    private void Awake()
    {
        StartCoroutine(ManageTimeRemaining());
    }

    public void EndGame()
    {
        OnEndGame.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
    }

    IEnumerator ManageTimeRemaining()
    {
        while (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        EndGame();
    }
}
