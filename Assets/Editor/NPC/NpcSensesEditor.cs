using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The editor for the NPC's senses. Draws the detection range, field of view, and hearing radius.
/// </summary>
[CustomEditor (typeof (NpcSenses))]
public class NpcSensesEditor : Editor
{
    private NpcSenses _npcSenses;
	private void OnSceneGUI() {
		_npcSenses = (NpcSenses)target;
        DrawHearingRadius();
        DrawFieldOfView();
        DrawDetectionLine();
	}
    /// <summary>
    /// Draws the NPC's hearing radius on screen, which is the NPC's auditory range.
    /// </summary>
    private void DrawHearingRadius()
    {
        Handles.color = new Color(1, 1, 0, 0.1f); // Yellow color with 50% transparency
        Handles.DrawSolidArc(_npcSenses.transform.position, Vector3.up, Vector3.forward, 360, _npcSenses.AuditoryRange);
    }
    /// <summary>
    /// Draws the NPC's field of view on screen, which is the NPC's sight range and field of view angle.
    /// </summary>
    private void DrawFieldOfView()
    {
        Vector3 viewAngle = _npcSenses.DirFromAngle(-_npcSenses.FieldOfViewAngle / 2, false);

        Handles.color = new Color(0, 0, 1, 0.2f); // Blue color with 50% transparency
        Handles.DrawSolidArc(_npcSenses.transform.position, Vector3.up, viewAngle, _npcSenses.FieldOfViewAngle, _npcSenses.SightRange);
    }
    /// <summary>
    /// Draws a line from the NPC to the detected clutter. The line is red if the clutter is visible, and yellow if the clutter is audible.
    /// </summary>
    private void DrawDetectionLine()
    {
		foreach (ObservableObject detectedObject in _npcSenses.DetectedObjects) {
            if(detectedObject.IsVisible)
            {
                Handles.color = Color.red;
            }
            if(detectedObject.IsAudible && !detectedObject.IsVisible)
            {
                Handles.color = Color.yellow;
            }

			Handles.DrawLine(_npcSenses.transform.position, detectedObject.transform.position);
		}
    }

}
