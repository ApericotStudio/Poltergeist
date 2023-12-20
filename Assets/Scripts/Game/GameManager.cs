using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate void TimeLeftChanged(int value);
    public event TimeLeftChanged OnTimePassedChanged;

    [HideInInspector] public UnityEvent OnEndGame = new UnityEvent();

    [Header("References")]
    [SerializeField] private GameObject _pauseCanvas;

    [Header("Adjustable variables")]
    [SerializeField] private int _timePassed;

    private void Awake()
    {
        StartCoroutine(ManageTimeLeft());
        _pauseCanvas.GetComponent<PauseController>().ResumeButton.onClick.AddListener(TogglePause);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (Time.timeScale == 0 && _pauseCanvas.activeSelf)
        {
            Time.timeScale = 1;
            _pauseCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            _pauseCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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
