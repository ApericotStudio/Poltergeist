using UnityEngine;
using UnityEngine.Events;

public class TutorialTest : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<string> updateTutorialText;

    [SerializeField]
    private UnityEvent tutorialOver;

    private bool updateNeeded = true;

    private void Update()
    {
        if (updateNeeded)
        {
            updateTutorialText.Invoke($"Time since startup: {(int)Time.realtimeSinceStartup}");
        }        

        if(Time.realtimeSinceStartup > 10f && updateNeeded)
        {
            tutorialOver.Invoke();
            updateNeeded = false;
        }
    }
}
