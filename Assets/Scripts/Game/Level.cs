using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public string Description;
    [Tooltip("Name of the scene that will be loaded")]
    public string SceneName;
}
