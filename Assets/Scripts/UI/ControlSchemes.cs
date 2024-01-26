using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSchemes : MonoBehaviour
{
    [SerializeField]
    private GameObject _mouseKeyboardControls;
    [SerializeField]
    private GameObject _controllerControls;
    [SerializeField]
    private Button _mouseKeyboardButton;
    [SerializeField]
    private Button _controllerButton;

    private void Start()
    {
        _mouseKeyboardButton.onClick.AddListener(OnMouseKeyboardButtonClicked);
        _controllerButton.onClick.AddListener(OnControllerButtonClicked);
    }

    private void OnMouseKeyboardButtonClicked()
    {
        _mouseKeyboardControls.SetActive(true);
        _controllerControls.SetActive(false);
    }

    private void OnControllerButtonClicked()
    {
        _mouseKeyboardControls.SetActive(false);
        _controllerControls.SetActive(true);
    }
}
