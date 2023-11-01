using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TutorialText : MonoBehaviour
{
    private TMP_Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the text displayed by the Text object.
    /// Call this function on an event.
    /// </summary>
    /// <param name="text">The new text for the TMPro text component</param>
    public void UpdateText(string newText)
    {
        textComponent.text = newText;
    }
    
    /// <summary>
    /// Hides the text.
    /// Call this function on an event.
    /// </summary>
    public void Hide()
    {
        textComponent.gameObject.SetActive(false);
    }

    public void Show()
    {
        textComponent.gameObject.SetActive(true);
    }
}
