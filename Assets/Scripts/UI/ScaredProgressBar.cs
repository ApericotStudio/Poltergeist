using UnityEngine;
using UnityEngine.UI;

public class AnxietyProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;
    private float anxiety;

    public void OnMinAnxiety()
    {

    }

    public void OnMaxAnxiety()
    {
        
    }

    public void OnValueUpdate(float anxiety)
    {
        this.anxiety = anxiety;
        fillImage.fillAmount = anxiety;
    }
}
