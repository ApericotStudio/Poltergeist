using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController _playerController;
    [SerializeField]
    private PolterSenseController _polterSenseController;
    [SerializeField]
    private PossessionController _possessionController;
    [SerializeField]
    private InteractController _interactController;

    private InGameUIController _uiController;

    private int _tutorialIndex;
    // Start is called before the first frame update
    void Start()
    {
        /*        _playerController = gameObject.GetComponent*/
        _uiController = gameObject.GetComponent<InGameUIController>();
        showTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_tutorialIndex)
        {
            case 1:
                
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
        }
    }

    private void showTutorial()
    {
        _uiController.ShowTutorial();
        ++_tutorialIndex;
    }
}
