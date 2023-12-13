using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelCatalog", menuName = "ScriptableObjects/LevelCatalog", order = 2)]
public class LevelCatalog : ScriptableObject
{
    public List<Level> Levels = new List<Level>();

    public Level GetCurrent()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        foreach(Level level in Levels)
        {
            if (level.SceneName == currentSceneName)
            {
                return level;
            }
        }
        throw new System.Exception("LevelCatalog does not contain a level corresponding to the current scene");
    }
}
