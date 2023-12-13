using TMPro;
using UnityEngine;

public class GradeDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _visitorsLeft;
    [SerializeField] private TextMeshProUGUI _differentObjectsUsed;
    [SerializeField] private TextMeshProUGUI _phobiaScares;
    [SerializeField] private TextMeshProUGUI _timeLeft;
    [SerializeField] private TextMeshProUGUI _result;

    public void SetGrade(Grade grade)
    {
        if (grade == null)
        {
            NoGrade();
            return;
        }
        _visitorsLeft.text = grade.VisitorsLeft.ToString();
        _differentObjectsUsed.text = grade.DifferentObjectsUsed.ToString();
        _phobiaScares.text = grade.PhobiaScares.ToString();
        _timeLeft.text = grade.TimeLeft.ToString();
        _result.text = grade.Result.ToString();
    }

    private void NoGrade()
    {
        _visitorsLeft.text = "0";
        _differentObjectsUsed.text = "0";
        _phobiaScares.text = "0";
        _timeLeft.text = "0";
        _result.text = "0";
    }
}
