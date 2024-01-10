using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController _playerController;
    private PolterSenseController _polterSenseController;
    private PossessionController _possessionController;
    private InteractController _interactController;

    [SerializeField]
    private InGameUIController _uiController;

    private int _tutorialIndex;
    private int _counter;
    private bool _tutorialShown;
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
        checkTutorialStep();

    }

    private void showTutorial(int index)
    {
        if(_counter == index + 1)
        {
            return;
        }

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

        Debug.Log(_counter);
    }

    private void unsubscribeEvents()
    {
        _polterSenseController.isEnabled -= showTutorial;
        _interactController.hasInteracted -= showTutorial;
        _playerController.hasMoved -= showTutorial;
    }

    private void checkTutorialStep()
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


}
