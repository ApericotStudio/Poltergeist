using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCatalog", menuName = "ScriptableObjects/LevelCatalog", order = 2)]
public class LevelCatalog : ScriptableObject
{
    public List<Level> Levels = new List<Level>();
}
