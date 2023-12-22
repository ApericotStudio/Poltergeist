public class Grade
{
    public Grade(GradeFile gradeFile)
    {
        VisitorsLeft = gradeFile.VisitorsLeft;
        DifferentObjectsUsed = gradeFile.DifferentObjectsUsed;
        PhobiaScares = gradeFile.PhobiaScares;
        TimePassed = gradeFile.TimePassed;
    }

    public Grade()
    {

    }

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
    public int TimePassed
    {
        get
        {
            return _timePassed;
        }
        set
        {
            _timePassed = value;
            _isDirty = true;
        }
    }

    private int _differentObjectsUsedScore
    {
        get
        {
            int totalAmountOfObjects = 30; // this is an estimate
            int objectsUsedPercentage = _differentObjectsUsed / totalAmountOfObjects * 100;
            if (objectsUsedPercentage >= 80) return 4;
            if (objectsUsedPercentage >= 50) return 3;
            if (objectsUsedPercentage >= 25) return 2;
            return 1;
        }
    }

    private int _phobiaScaresScore
    {
        get
        {
            if (_phobiaScares >= 9) return 4;
            if (_phobiaScares >= 4) return 3;
            if (_phobiaScares >= 1) return 2;
            return 1;
        }
    }

    private int _timePassedScore
    {
        get
        {
            if (_timePassed <= 90) return 4;
            if (_timePassed <= 180) return 3;
            if (_timePassed <= 300) return 2;
            return 1;
        }
    }

    private int CalculateResult()
    {
        int totalScore = _differentObjectsUsedScore + _phobiaScaresScore + _timePassedScore / 3;
        return totalScore;
    }
}
