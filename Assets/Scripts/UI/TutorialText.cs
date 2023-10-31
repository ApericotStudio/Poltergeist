using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TutorialText : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the text displayed by the Text object.
    /// Call this function on an event.
    /// </summary>
    /// <param name="text">The new text for the </param>
    public void UpdateText(string text)
    {
        this.text.text = text;
    }
    
    /// <summary>
    /// Hides the text.
    /// Call this function on an event.
    /// </summary>
    public void Hide()
    {
        text.gameObject.SetActive(false);
    }
}
