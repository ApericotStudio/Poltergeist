using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DeveloperConsoleBehaviour : MonoBehaviour
{
    [SerializeField] private string _prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] _commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] private GameObject _uiCanvas = null;
    [SerializeField] private TMP_InputField _inputField = null;

    private float _pausedTimeScale;

    private DeveloperConsole _developerConsole;

    public DeveloperConsole DeveloperConsole
    {
        get
        {
            if (_developerConsole != null) { return _developerConsole; }
            return _developerConsole = new DeveloperConsole(_prefix, _commands);
        }
    }

    public void Toggle()
    {
        if (_uiCanvas.activeSelf)
        {
            Time.timeScale = _pausedTimeScale;
            _uiCanvas.SetActive(false);
        }
        else
        {
            _pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            _uiCanvas.SetActive(true);
            _inputField.ActivateInputField();
        }
    }

    public void ProcessCommand(string inputValue)
    {
        DeveloperConsole.ProcessCommand(inputValue);

        _inputField.text = string.Empty;
    }
}
