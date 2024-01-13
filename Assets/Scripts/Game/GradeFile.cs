using System.Collections.Generic;

public class GradeFile
{
    public GradeFile(int result, int visitorsLeft, int differentObjectsUsed, int timePassed, List<GradeCriteria> gradeCriterias)
    {
        Result = result;
        VisitorsLeft = visitorsLeft;
        DifferentObjectsUsed = differentObjectsUsed;
        PhobiaScares = 0;
        TimePassed = timePassed;
        GradeCriterias = gradeCriterias;
    }

    public int Result;
    public int VisitorsLeft;
    public int DifferentObjectsUsed;
    public int PhobiaScares;
    public int TimePassed;

    public List<GradeCriteria> GradeCriterias;
}
