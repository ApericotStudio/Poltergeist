using UnityEngine;

[CreateAssetMenu(fileName = "Grade", menuName = "ScriptableObjects/Grade", order = 1)]
public class Grade : ScriptableObject
{
    private bool _isDirty = true;
    private int _result;
    public int Result
    {
        get
        {
            if (_isDirty)
            {
                _result = CalculateResult();
                _isDirty = false;
            }
            return _result;
        }
    }

    private int _visitorsLeft = 0;
    private int _differentObjectsUsed = 0;
    private int _phobiaScares = 0;
    private float _timeLeft = 0;

    public int VisitorsLeft
    {
        get
        {
            return _visitorsLeft;
        }
        set
        {
            _visitorsLeft = value;
            _isDirty = true;
        }
    }
    public int DifferentObjectsUsed
    {
        get
        {
            return _differentObjectsUsed;
        }
        set
        {
            _differentObjectsUsed = value;
            _isDirty = true;
        }
    }
    public int PhobiaScares
    {
        get
        {
            return _phobiaScares;
        }
        set
        {
            _phobiaScares = value;
            _isDirty = true;
        }
    }
    public float TimeLeft
    {
        get
        {
            return _timeLeft;
        }
        set
        {
            _timeLeft = value;
            _isDirty = true;
        }
    }

    private int CalculateResult()
    {
        int score = 0;
        score -= _visitorsLeft * 20;
        score += _differentObjectsUsed * 5;
        score += PhobiaScares * 10;
        score += (int) _timeLeft;
        return score;
    }
}
