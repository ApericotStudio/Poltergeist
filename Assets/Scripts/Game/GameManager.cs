using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate void TimeLeftChanged(float value);
    public event TimeLeftChanged OnTimeLeftChanged;

    [HideInInspector] public UnityEvent OnEndGame = new UnityEvent();

    [Header("Adjustable variables")]
    [SerializeField] private float _timeLeft;

    private void Awake()
    {
        StartCoroutine(ManageTimeLeft());
    }

    public void EndGame()
    {
        OnEndGame.Invoke();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        StopCoroutine(ManageTimeLeft());
    }

    IEnumerator ManageTimeLeft()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            OnTimeLeftChanged?.Invoke(_timeLeft);
            yield return new WaitForFixedUpdate();
        }
        EndGame();
    }
}
