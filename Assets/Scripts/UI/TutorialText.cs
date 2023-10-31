using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TutorialText : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateText(string text)
    {
        this.text.text = text;
    }
    
    public void Hide()
    {
        text.gameObject.SetActive(false);
    }
}
