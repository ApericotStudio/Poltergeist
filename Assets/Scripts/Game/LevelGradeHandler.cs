using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGradeHandler
{
    private string _gradeDirPath = Application.persistentDataPath;

    public Grade Load(string sceneName)
    {
        string fullPath = Path.Combine(_gradeDirPath, sceneName);
        Grade grade = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                GradeFile gradeFile = JsonUtility.FromJson<GradeFile>(dataToLoad);
                grade = new Grade(gradeFile);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file " + fullPath + "\n" + e);
            }
        }
        return grade;
    }

    public void Save(Grade grade, string sceneName)
    {
        string fullPath = Path.Combine(_gradeDirPath, sceneName);
        
        List<GradeCriteria> gradeCriterias = new List<GradeCriteria>();
        gradeCriterias.Add(grade.GradeObjects);
        if (grade.GradePhobias != null) gradeCriterias.Add(grade.GradePhobias);
        gradeCriterias.Add(grade.GradeTime);


        GradeFile gradeToStore = new GradeFile(grade.Result, grade.VisitorsLeft, grade.DifferentObjectsUsed, grade.PhobiaScares, grade.TimePassed);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gradeToStore, true);
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.Write(dataToStore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file " + fullPath + "\n" + e);
        }
    }
}
