using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Card Message", menuName = "ScriptableObjects/Tutorial Card Message", order = 1)]
public class TutorialCardMessage : ScriptableObject
{
    public string Title;
    public string Description;
}
