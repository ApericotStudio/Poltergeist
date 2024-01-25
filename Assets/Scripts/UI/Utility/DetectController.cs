using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DetectController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    private GameObject _previousSelectedObject;
    [SerializeField]
    private bool _isMainMenu = false;

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != null)
        {
            _previousSelectedObject = EventSystem.current.currentSelectedGameObject;
        }
        if (_playerInput.currentControlScheme == "Gamepad")
        {
            EventSystem.current.SetSelectedGameObject(_previousSelectedObject);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            if(Time.timeScale == 0 || _isMainMenu)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
