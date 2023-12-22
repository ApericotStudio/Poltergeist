using UnityEngine;

[CreateAssetMenu(fileName = "New Grade Converter", menuName = "ScriptableObjects/Grade Converter", order = 1)]
public class GradeConverter : ScriptableObject
{
    [SerializeField] private Sprite _grade_a;
    [SerializeField] private Sprite _grade_b;
    [SerializeField] private Sprite _grade_c;
    [SerializeField] private Sprite _grade_d;

    public Sprite GetGradeSprite(int score)
    {
        if (score == 4) return _grade_a;
        if (score == 3) return _grade_b;
        if (score == 2) return _grade_c;
        return _grade_d;
    }
}
