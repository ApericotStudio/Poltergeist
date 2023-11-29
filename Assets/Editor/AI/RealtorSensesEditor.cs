using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (RealtorSenses))]
public class RealtorSensesEditor : Editor
{
    private RealtorSenses _realtorSenses;

    private void OnSceneGUI()
    {
        _realtorSenses = (RealtorSenses)target;
        DrawDetectionRadius();
        DrawDetectionLines();
    }

    /// <summary>
    /// Draws the realtor's detection radius on screen.
    /// </summary>
    private void DrawDetectionRadius()
    {
        Handles.color = Handles.color = new Color(0, 1, 0, 0.1f);
        Handles.DrawSolidArc(_realtorSenses.transform.position, Vector3.up, Vector3.forward, 360, _realtorSenses.DetectionRange);
    }

    /// <summary>
    /// Draws a line from the realtor to the detected NPC. 
    /// </summary>
    private void DrawDetectionLines()
    {
        foreach (var npc in _realtorSenses.DetectedNpcs)
        {
            Handles.color = Color.blue;
            Handles.DrawLine(_realtorSenses.transform.position, npc.transform.position);
        }
    }
}
