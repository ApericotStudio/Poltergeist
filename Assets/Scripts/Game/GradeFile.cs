public class GradeFile
{
    public GradeFile(int result, int visitorsLeft, int differentObjectsUsed, int timeLeft)
    {
        Result = result;
        VisitorsLeft = visitorsLeft;
        DifferentObjectsUsed = differentObjectsUsed;
        PhobiaScares = 0;
        TimeLeft = timeLeft;
    }

    public int Result;
    public int VisitorsLeft;
    public int DifferentObjectsUsed;
    public int PhobiaScares;
    public float TimeLeft;
}
