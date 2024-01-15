using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate void TimeLeftChanged(int value);
    public event TimeLeftChanged OnTimePassedChanged;

    public delegate void PauseToggled(bool isPaused);
    public event PauseToggled OnPauseToggled;

    [HideInInspector] public UnityEvent OnEndGame = new UnityEvent();

    [Header("References")]
    [SerializeField] private GameObject _pauseCanvas;

    [Header("Adjustable variables")]
    [SerializeField] private int _timePassed;

    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        float volume = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _audioMixer.SetFloat("GameVol", volume);

        int fullscreen = PlayerPrefs.GetInt(PlayerPrefsVariable.Fullscreen.ToString(), 1);
        if (fullscreen == 1)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        StartCoroutine(ManageTimeLeft());
        _pauseCanvas.GetComponent<PauseController>().ResumeButton.onClick.AddListener(TogglePause);
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat(PlayerPrefsVariable.Volume.ToString(), 1);
        _audioMixer.SetFloat("GameVol", Mathf.Log10(volume) * 20);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        UpdateCursor();
    }

    public void onButtonStart()
    {
        TogglePause();
    }

    private void UpdateCursor()
    {
        if(Time.timeScale == 0 && _pauseCanvas.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Time.timeScale == 1)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void TogglePause()
    {
        if (Time.timeScale == 0 && _pauseCanvas.activeSelf)
        {
            PlayerPrefs.Save();
            Time.timeScale = 1;
            AudioListener.pause = false;
            _pauseCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            OnPauseToggled?.Invoke(false);
        }
        else if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            _pauseCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OnPauseToggled?.Invoke(true);
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
