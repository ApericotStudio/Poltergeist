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
        switch(gradeCriterias.Count)
        {
            case 2:
                ObjectsCriteria = new List<float> { gradeCriterias[0].Criteria_A, gradeCriterias[0].Criteria_B, gradeCriterias[0].Criteria_C };
                TimeCriteria = new List<float> { gradeCriterias[1].Criteria_A, gradeCriterias[1].Criteria_B, gradeCriterias[1].Criteria_C };
                break;
            case 3:
                ObjectsCriteria = new List<float> { gradeCriterias[0].Criteria_A, gradeCriterias[0].Criteria_B, gradeCriterias[0].Criteria_C };
                PhobiasCriteria = new List<float> { gradeCriterias[1].Criteria_A, gradeCriterias[1].Criteria_B, gradeCriterias[1].Criteria_C };
                TimeCriteria = new List<float> { gradeCriterias[2].Criteria_A, gradeCriterias[2].Criteria_B, gradeCriterias[2].Criteria_C };
                break;

        }
    }

    public int Result;
    public int VisitorsLeft;
    public int DifferentObjectsUsed;
    public int PhobiaScares;
    public int TimePassed;

    public List<float> ObjectsCriteria;
    public List<float> PhobiasCriteria;
    public List<float> TimeCriteria;
}
