using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Tutorial : MonoBehaviour
{
    private TMP_Text tutorialText;

    private void Start()
    {
        tutorialText = GetComponent<TMP_Text>();
    }

    public void UpdateText(string newText)
    {
        tutorialText.text = newText;
    }

    public void Hide()
    {
        tutorialText.gameObject.SetActive(false);
    }

    public void Show()
    {
        tutorialText.gameObject.SetActive(true);
    }
}
