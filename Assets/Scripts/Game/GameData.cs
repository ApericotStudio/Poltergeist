using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int AmountOfVisitors;
    public int AmountOfVisitorsScared;
    public string Grade
    {
        get
        {
            if (AmountOfVisitorsScared == AmountOfVisitors)
            {
                return "Passed";
            }
            return "Failed";
        }
    }
}
