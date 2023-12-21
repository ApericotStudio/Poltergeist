using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "ScriptableObjects/SettingsData", order = 1)]
public class SettingsData : ScriptableObject
{
    public float Sensitivity = 1.0f;
    public float SoundVolume = 1.0f;
    public float MusicVolume = 1.0f;
}
