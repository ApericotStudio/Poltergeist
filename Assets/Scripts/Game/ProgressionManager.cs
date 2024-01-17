using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] GameObject GradHat;
    LevelGradeHandler gradeHandler;
    // Start is called before the first frame update
    void Start()
    {
        gradeHandler = new LevelGradeHandler();
        Grade assignmentGrade = gradeHandler.Load("Assignment");
        Grade examGrade = gradeHandler.Load("FinalExam");

        if (assignmentGrade != null && examGrade != null)
        {
            if (assignmentGrade.Result == 4 && examGrade.Result == 4)
            {
                GetGradHat();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetGradHat()
    {
        GameObject headbone = GameObject.Find("headbone");
        if (headbone != null)
        {
            GameObject coolHat = Instantiate(original: GradHat, parent: headbone.transform, position: headbone.transform.position, rotation: Quaternion.identity);
            coolHat.transform.forward = headbone.transform.forward;
        }
    }
}
