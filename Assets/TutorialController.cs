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
    private FearHandler _fearHandler;
    private VisitorManager _visitorManager;
    private PolterSenseController _polterSenseController;
    private PossessionController _possessionController;
    private InteractController _interactController;
    private FearHandler[] _visitors;

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
        _visitorManager = gameObject.GetComponent<VisitorManager>();
        _visitors = _visitorManager.VisitorCollection.GetComponentsInChildren<FearHandler>();

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
        foreach (FearHandler visitor in _visitors)
        {
            visitor.activatedPhobia -= showTutorial;
        }
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
                foreach (FearHandler visitor in _visitors)
                {
                    visitor.activatedPhobia += showTutorial;
                }
                break;
        }
    }
}
