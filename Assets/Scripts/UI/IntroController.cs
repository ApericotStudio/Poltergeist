using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName;
    [SerializeField] private Animator _canvasAnimator;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _canvasAnimator.SetTrigger("Pressed");
        }
    }

    public void OnAnimationFinish()
    {
        this.gameObject.SetActive(false);
    }
}
