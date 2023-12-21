using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }
}
