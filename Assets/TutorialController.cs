using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController _playerController;
    [SerializeField]
    private FearHandler _fearHandler;
    private PolterSenseController _polterSenseController;
    private PossessionController _possessionController;
    private InteractController _interactController;

    [SerializeField]
    private InGameUIController _uiController;

    private int _tutorialIndex;
    private int _counter;
    private bool _firstTutorialShown = false;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Assignment")
        {
            _firstTutorialShown = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _polterSenseController = _playerController.GetComponent<PolterSenseController>();
        _possessionController = _playerController.GetComponent<PossessionController>();
        _interactController = _playerController.GetComponent<InteractController>();

        showTutorial(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_firstTutorialShown)
        {
            checkFirstTutorial();
        }
        else
        {
            checkSecondTutorial();
        }
    }

    private void showTutorial(int index)
    {
        if(_counter == index + 1)
        {
            return;
        }

        //previous tutorial popups don't appear again
        if (index <= _counter - 2)
        {
            return;
        }

        _uiController.ShowTutorial(index);
        _counter = index + 1;
        ++_tutorialIndex;

        Debug.Log(index);
        Debug.Log(_counter);

        unsubscribeEvents();
    }

    private void unsubscribeEvents()
    {
        _polterSenseController.isEnabled -= showTutorial;
        _interactController.hasInteracted -= showTutorial;
        _playerController.hasMoved -= showTutorial;
        _possessionController.hasPossessed -= showTutorial;
        _fearHandler.activatedPhobia -= showTutorial;
    }

    private void checkFirstTutorial()
    {
        switch (_counter)
        {
            case 1:
                _playerController.hasMoved += showTutorial;
                break;
            case 2:
                _polterSenseController.isEnabled += showTutorial;
                break;
            case 3:
                _interactController.hasInteracted += showTutorial;
                break;
            case 4:
                _possessionController.hasPossessed += showTutorial;
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
        }
    }

    private void checkSecondTutorial()
    {
        switch (_counter)
        {
            case 0:
                break;
            case 1:
                _fearHandler.activatedPhobia += showTutorial;
                break;
        }
    }
}
