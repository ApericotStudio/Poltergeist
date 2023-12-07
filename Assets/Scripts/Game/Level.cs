using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public string Description;
    public Grade Grade;
    [Tooltip("Name of the scene that will be loaded")]
    public string SceneName;
}
