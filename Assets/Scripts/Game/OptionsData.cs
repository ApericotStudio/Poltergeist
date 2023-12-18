using UnityEngine;

[CreateAssetMenu(fileName = "Options Data", menuName = "ScriptableObjects/Options Data", order = 1)]
public class OptionsData : ScriptableObject
{
    public float Volume = 1.0f;
    public float Sensetivity = 1.0f;
    public float Brightness = 1.0f;
    public float Contrast = 1.0f;
    public bool Fullscreen = true;
}
