using UnityEngine;
using UnityEngine.Events;

public class TutorialTest : MonoBehaviour
{
    [Header("Test Events")]
    [SerializeField]
    [Tooltip("The event that is raised when the text should change")]
    private UnityEvent<string> updateTutorialText;

    [SerializeField]
    [Tooltip("The event that is raised when the text should be hidden")]
    private UnityEvent tutorialOver;

    private void Update()
    {        
        UpdateText();
    }

    /// <summary>
    /// Updates the tutorial text on screen to show the time.
    /// When 10 seconds has passed the text hides itself. 
    /// </summary>
    private void UpdateText()
    {
        updateTutorialText.Invoke($"Time since startup: {Time.realtimeSinceStartup}");

        if(Time.realtimeSinceStartup > 10f)
        {
            tutorialOver.Invoke();
            updateTutorialText.RemoveAllListeners();
        }
    }
}
