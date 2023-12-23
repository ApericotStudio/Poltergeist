using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    [Tooltip("Title shown in level select")]
    public string Title;
    [Tooltip("Description shown in level select")]
    public string Description;
    [Tooltip("Name of the scene that will be loaded")]
    public string SceneName;
}
