using UnityEngine;

[CreateAssetMenu(fileName = "Grade", menuName = "ScriptableObjects/Grade", order = 1)]
public class Grade : ScriptableObject
{
    private bool _isDirty = true;
    private string _result;
    public string Result
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

    private int _visitorsLeft;
    private int _differentObjectsUsed;
    private int _phobiaScares;
    private int _timeLeft;

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
    public int TimeLeft
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

    private string CalculateResult()
    {
        if(_visitorsLeft == 0)
        {
            return "passed";
        }
        return "failed";
    }
}
