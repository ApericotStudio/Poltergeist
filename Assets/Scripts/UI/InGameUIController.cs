using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    /* To-do:
     * - Scale notification to proper size
     */

    [SerializeField] private Transform _notificationParent;
    [SerializeField] private GameObject _notificationPrefab;

    [SerializeField] private GameObject _tutorialCard;
    [SerializeField] private GameObject _tutorialCardNext;
    [SerializeField] private TextMeshProUGUI _tutorialCardTitle;
    [SerializeField] private TextMeshProUGUI _tutorialCardDescription;
    [SerializeField] private List<TutorialCardMessage> _tutorialCardMessages = new();
    private int _tutorialCardIndex = 0;
    private string _currentSceneName;
    private readonly string _mainGameScene = "FinalExam";

    [SerializeField] private Transform _visitorOverlayParent;
    [SerializeField] private GameObject _visitorCollection;

    private void Awake()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
        AddNpcOverlays();
    }

    private void Update()
    {
        NextTutorial();
    }

    public void ShowTutorial(int index)
    {
        if (_tutorialCardMessages.Count > index)
        {
            SetTutorial(_tutorialCardMessages[index]);
            _tutorialCardIndex = index;

            if (index > 6 || _currentSceneName == _mainGameScene || index == 3)
            {
                _tutorialCardNext.SetActive(true);
            }
            else
            {
                _tutorialCardNext.SetActive(false);
            }
        }
        else
        {
            StopTutorial();
        }
    }

    public void StopTutorial()
    {
        _tutorialCard.SetActive(false);
        _tutorialCardNext.SetActive(false);
    }

    private void NextTutorial()
    {
        if(!(_tutorialCardMessages.Count > _tutorialCardIndex))
        {
            return;
        }

/*        if (Input.GetKeyDown(KeyCode.R))
        {
            if(_tutorialCardIndex > 5 || _tutorialCardIndex == 3)
            {
                ShowTutorial(_tutorialCardIndex + 1);
            }

            if(_currentSceneName == _mainGameScene)
            {
                _tutorialCard.SetActive(false);
            }
        }*/
    }

    public void GoToNextTutorial()
    {
        if (_tutorialCardIndex > 5 || _tutorialCardIndex == 3)
        {
            ShowTutorial(_tutorialCardIndex + 1);
        }

        if (_currentSceneName == _mainGameScene)
        {
            _tutorialCard.SetActive(false);
        }
    }

    public void ShowNotification(string text, float duration)
    {
        GameObject notification = Instantiate(_notificationPrefab, _notificationParent);
        notification.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Destroy(notification, duration);
    }

    public void SetTutorial(TutorialCardMessage tutorialCardMessage)
    {
        _tutorialCard.SetActive(true);
        _tutorialCardTitle.text = tutorialCardMessage.Title;
        _tutorialCardDescription.text = tutorialCardMessage.Description;
    }

    public void AddNpcOverlays()
    {
        foreach (VisitorController controller in _visitorCollection.GetComponentsInChildren<VisitorController>())
        {
            GameObject visitorOverlay = Instantiate(controller.VisitorOverlayPrefab, _visitorOverlayParent);
            visitorOverlay.GetComponent<VisitorOverlayController>().Setup(controller, this);
        }
    }
}
