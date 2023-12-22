using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    /* To-do:
     * - Scale notification to proper size
     */

    [SerializeField] private Transform _notificationParent;
    [SerializeField] private GameObject _notificationPrefab;

    [SerializeField] private GameObject _tutorialCard;
    [SerializeField] private TextMeshProUGUI _tutorialCardTitle;
    [SerializeField] private TextMeshProUGUI _tutorialCardDescription;
    [SerializeField] private List<TutorialCardMessage> _tutorialCardMessages = new();
    private int _tutorialCardIndex = 0;

    [SerializeField] private Transform _visitorOverlayParent;
    [SerializeField] private GameObject _visitorCollection;

    private void Awake()
    {
        AddNpcOverlays();
    }

    private void Update()
    {
        //TestTutorialAndNotifications();
    }

    private void TestTutorialAndNotifications()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowNotification("I can't believe you've done this", 3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_tutorialCardMessages.Count > _tutorialCardIndex)
            {
                SetTutorial(_tutorialCardMessages[_tutorialCardIndex]);
                _tutorialCardIndex++;
            }
            else
            {
                _tutorialCard.SetActive(false);
            }
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
            visitorOverlay.GetComponent<VisitorOverlayController>().Setup(controller);
        }
    }
}