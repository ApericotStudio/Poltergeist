using UnityEngine;

public class Grade
{
    public GradeCriteria GradeObjects;
    public GradeCriteria GradeTime;
    public GradeCriteria GradePhobias;

    public Grade(GradeFile gradeFile)
    {
        _result = gradeFile.Result;
        _isResultCalculated = true;
        VisitorsLeft = gradeFile.VisitorsLeft;
        DifferentObjectsUsed = gradeFile.DifferentObjectsUsed;
        PhobiaScares = gradeFile.PhobiaScares;
        TimePassed = gradeFile.TimePassed;
    }

    public Grade()
    {

    }

    private bool _isResultCalculated = false;
    private int _result;
    public int Result
    {
        get
        {
            if (!_isResultCalculated)
            {
                _result = CalculateResult();
                _isResultCalculated = false;
            }
            return _result;
        }
    }

    private int _visitorsLeft = 0;
    private int _differentObjectsUsed = 0;
    private int _phobiaScares = 0;
    private int _timePassed = 0;

    public int VisitorsLeft
    {
        get
        {
            return _visitorsLeft;
        }
        set
        {
            _visitorsLeft = value;
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
        }
    }
    public int TimePassed
    {
        get
        {
            return _timePassed;
        }
        set
        {
            _timePassed = value;
        }
    }

    private int _differentObjectsUsedScore
    {
        get
        {
            if (_differentObjectsUsed >= GradeObjects.Criteria_A) return 4;
            if (_differentObjectsUsed >= GradeObjects.Criteria_B) return 3;
            if (_differentObjectsUsed >= GradeObjects.Criteria_C) return 2;
            return 1;
        }
    }


    public int DifferentObjectsUsedScore
    {
        get
        {
            return _differentObjectsUsedScore;
        }
    }

    private int _phobiaScaresScore
    {
        get
        {
            if (!GradePhobias)
            {
                return 0;
            }

            if (_phobiaScares >= GradePhobias.Criteria_A) return 4;
            if (_phobiaScares >= GradePhobias.Criteria_B) return 3;
            if (_phobiaScares >= GradePhobias.Criteria_C) return 2;
            return 1;
        }
    }

    public int PhobiaScaresScore
    {
        get
        {
            return _phobiaScaresScore;
        }
    }

    private int _timePassedScore
    {
        get
        {
            if (_timePassed <= GradeTime.Criteria_A) return 4;
            if (_timePassed <= GradeTime.Criteria_B) return 3;
            if (_timePassed <= GradeTime.Criteria_C) return 2;
            return 1;
        }
    }

    public int TimePassedScore
    {
        get
        {
            return _timePassedScore;
        }
    }

    private int CalculateResult()
    {
        int _divideBy;

        if (_phobiaScaresScore == 0)
        {
            _divideBy = 2;
        }
        else
        {
            _divideBy = 3;
        }

        int totalScore = (_differentObjectsUsedScore + _phobiaScaresScore + _timePassedScore) / _divideBy;
        return totalScore;
    }
}
