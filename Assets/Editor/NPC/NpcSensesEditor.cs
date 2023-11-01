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
        DrawDetectionRange();
        DrawFieldOfView();
        DrawHearingRadius();
        DrawDetectionLine();
	}
    /// <summary>
    /// Draws the NPC's detection range on screen, which is the maximum of the NPC's hearing radius and field of view.
    /// </summary>
    private void DrawDetectionRange()
    {
        Handles.color = Color.gray;
        Handles.DrawWireArc(_npcSenses.transform.position, Vector3.up, Vector3.forward, 360,  Math.Max(_npcSenses.AuditoryRange, _npcSenses.DetectionRange));
    }
    /// <summary>
    /// Draws the NPC's hearing radius on screen, which is the NPC's auditory range.
    /// </summary>
    private void DrawHearingRadius()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(_npcSenses.transform.position, Vector3.up, Vector3.forward, 360, _npcSenses.AuditoryRange);
    }
    /// <summary>
    /// Draws the NPC's field of view on screen, which is the NPC's sight range and field of view angle.
    /// </summary>
    private void DrawFieldOfView()
    {
        Vector3 viewAngleA = _npcSenses.DirFromAngle (-_npcSenses.FieldOfViewAngle / 2, false);
		Vector3 viewAngleB = _npcSenses.DirFromAngle (_npcSenses.FieldOfViewAngle / 2, false);

        Handles.color = Color.blue;
		Handles.DrawLine (_npcSenses.transform.position, _npcSenses.transform.position + viewAngleA * _npcSenses.SightRange);
		Handles.DrawLine (_npcSenses.transform.position, _npcSenses.transform.position + viewAngleB * _npcSenses.SightRange);
        Handles.DrawWireArc(_npcSenses.transform.position, Vector3.up, viewAngleA * _npcSenses.SightRange, _npcSenses.FieldOfViewAngle, _npcSenses.SightRange);
    }
    /// <summary>
    /// Draws a line from the NPC to the detected clutter. The line is red if the clutter is visible, and yellow if the clutter is audible.
    /// </summary>
    private void DrawDetectionLine()
    {
		foreach (Clutter detectedClutter in _npcSenses.DetectedClutter) {
            if(detectedClutter.IsVisible)
            {
                Handles.color = Color.red;
            }
            if(detectedClutter.IsAudible && !detectedClutter.IsVisible)
            {
                Handles.color = Color.yellow;
            }

			Handles.DrawLine(_npcSenses.transform.position, detectedClutter.transform.position);
		}
    }

}
