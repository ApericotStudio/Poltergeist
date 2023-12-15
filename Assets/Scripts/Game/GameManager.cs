using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate void TimeLeftChanged(int value);
    public event TimeLeftChanged OnTimePassedChanged;

    [HideInInspector] public UnityEvent OnEndGame = new UnityEvent();

    [Header("Adjustable variables")]
    [SerializeField] private int _timePassed;

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
        while (true)
        {
            yield return new WaitForSeconds(1);
            _timePassed++;
            OnTimePassedChanged?.Invoke(_timePassed);
        }
    }
}
