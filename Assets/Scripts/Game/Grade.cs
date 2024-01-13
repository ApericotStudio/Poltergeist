using UnityEngine;

public class Grade
{
    public GradeCriteria GradeObjects;
    public GradeCriteria GradeTime;
    public GradeCriteria GradePhobias;

    public Grade(GradeFile gradeFile)
    {
        VisitorsLeft = gradeFile.VisitorsLeft;
        DifferentObjectsUsed = gradeFile.DifferentObjectsUsed;
        PhobiaScares = gradeFile.PhobiaScares;
        TimePassed = gradeFile.TimePassed;

        // Get grading from file
        GradeObjects = ScriptableObject.CreateInstance<GradeCriteria>();
        GradeObjects.Criteria_A = gradeFile.ObjectsCriteria[0];
        GradeObjects.Criteria_B = gradeFile.ObjectsCriteria[1];
        GradeObjects.Criteria_C = gradeFile.ObjectsCriteria[2];

        GradeTime = ScriptableObject.CreateInstance<GradeCriteria>();
        GradeTime.Criteria_A = gradeFile.TimeCriteria[0];
        GradeTime.Criteria_B = gradeFile.TimeCriteria[1];
        GradeTime.Criteria_C = gradeFile.TimeCriteria[2];

        if(gradeFile.PhobiasCriteria != null && gradeFile.PhobiasCriteria.Count == 3)
        {
            GradePhobias = ScriptableObject.CreateInstance<GradeCriteria>();
            GradePhobias.Criteria_A = gradeFile.PhobiasCriteria[0];
            GradePhobias.Criteria_B = gradeFile.PhobiasCriteria[1];
            GradePhobias.Criteria_C = gradeFile.PhobiasCriteria[2];
        }
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
            if (_differentObjectsUsed >= GradeObjects.Criteria_A) return 4;
            if (_differentObjectsUsed >= GradeObjects.Criteria_B) return 3;
            if (_differentObjectsUsed >= GradeObjects.Criteria_C) return 2;
            return 1;
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
