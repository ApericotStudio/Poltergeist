using UnityEngine;
using UnityEngine.UI;

public class AnxietyProgressBar : MonoBehaviour
{
    public Slider slider;

    [SerializeField]
    [Tooltip("The inidicator image that gets shown when anxiety is low")]
    private GameObject lowAnxietyIndicator;

    [SerializeField]
    [Tooltip("The inidicator image that gets shown when anxiety is medium")]
    private GameObject mediumAnxietyIndicator;

    [SerializeField]
    [Tooltip("The inidicator image that gets shown when anxiety is high")]
    private GameObject highAnxietyIndicator;

    private float maxAnxiety = 100;

    /// <summary>
    /// Updates the Anxiety Meter based on the new anxiety value
    /// </summary>
    /// <param name="anxiety">The new value for anxiety</param>
    public void OnValueUpdate(float anxiety)
    {
        slider.value = anxiety/maxAnxiety;

        if(anxiety <= 25f)
        {
            lowAnxietyIndicator.SetActive(true);
            mediumAnxietyIndicator.SetActive(false);
            highAnxietyIndicator.SetActive(false);
        }
        else if(anxiety >= 75f)
        {
            lowAnxietyIndicator.SetActive(false);
            mediumAnxietyIndicator.SetActive(false);
            highAnxietyIndicator.SetActive(true);
        }
        else
        {
            lowAnxietyIndicator.SetActive(false);
            mediumAnxietyIndicator.SetActive(true);
            highAnxietyIndicator.SetActive(false);
        }
    }
}
