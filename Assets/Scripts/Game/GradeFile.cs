using System.Collections.Generic;

public class GradeFile
{
    public GradeFile(int result, int visitorsLeft, int differentObjectsUsed, int phobiaScares, int timePassed)
    {
        Result = result;
        VisitorsLeft = visitorsLeft;
        DifferentObjectsUsed = differentObjectsUsed;
        PhobiaScares = phobiaScares;
        TimePassed = timePassed;
    }

    public int Result;
    public int VisitorsLeft;
    public int DifferentObjectsUsed;
    public int PhobiaScares;
    public int TimePassed;
}
