using UnityEditor;
using UnityEngine;

/// <summary>
/// The editor for the NPC's senses. Draws the detection range, field of view, and hearing radius.
/// </summary>
[CustomEditor (typeof (AiDetection), true)]
public class AiDetectionEditor : Editor
{
    private AiDetection _aiDetection;
    private RealtorSenses _realtorSenses;

    private void OnEnable() {
        _aiDetection = (AiDetection)target;
        _realtorSenses = _aiDetection.GetComponent<RealtorSenses>();
    }
	private void OnSceneGUI() {
		_aiDetection = (AiDetection)target;
        DrawHearingRadius();
        DrawFieldOfView();
        DrawObjectDetectionLine();
        if(IsRealtor())
        {
            DrawFearReductionRange();
            DrawNpcDetectionLines();
        }
	}

    /// <summary>
    /// Draws the radius around the realtor that reduces NPC fear.
    /// </summary>
    private void DrawFearReductionRange()
    {
        if(_realtorSenses != null)
        {
            Handles.color = Handles.color = new Color(0, 1, 0, 0.1f);
            Handles.DrawSolidArc(_realtorSenses.transform.position, Vector3.up, Vector3.forward, 360, _realtorSenses.FearReductionRange);
        }
    }

    /// <summary>
    /// Draws the AI's hearing radius on screen, which is the AI's auditory range.
    /// </summary>
    private void DrawHearingRadius()
    {
        Handles.color = new Color(1, 1, 0, 0.1f); // Yellow color with 50% transparency
        Handles.DrawSolidArc(_aiDetection.transform.position, Vector3.up, Vector3.forward, 360, _aiDetection.AuditoryRange);
    }

    /// <summary>
    /// Draws the AI's field of view on screen, which is the AI's sight range and field of view angle.
    /// </summary>
    private void DrawFieldOfView()
    {
        Vector3 viewAngle = _aiDetection.DirFromAngle(-_aiDetection.FieldOfViewAngle / 2, false);

        Handles.color = new Color(0, 0, 1, 0.2f); // Blue color with 50% transparency
        Handles.DrawSolidArc(_aiDetection.transform.position, Vector3.up, viewAngle, _aiDetection.FieldOfViewAngle, _aiDetection.SightRange);
    }

    /// <summary>
    /// Draws a line from the AI to the detected clutter. The line is red if the clutter is visible, and yellow if the clutter is audible.
    /// </summary>
    private void DrawObjectDetectionLine()
    {
        foreach (var (observableObject, detectedProperties) in _aiDetection.DetectedObjects) {
            if(detectedProperties.IsVisible)
            {
                Handles.color = Color.blue;
                Handles.DrawLine(_aiDetection.transform.position, observableObject.transform.position);
            }
            else if(detectedProperties.IsAudible)
            {
                Handles.color = Color.yellow;
                Handles.DrawLine(_aiDetection.transform.position, observableObject.transform.position);
            }
        }
    }

    /// <summary>
    /// Draws a line from the realtor to the detected NPC. 
    /// </summary>
    private void DrawNpcDetectionLines()
    {
        foreach (var npc in _realtorSenses.DetectedNpcs)
        {
            Handles.color = Handles.color = new Color(0, 1, 0);
            Handles.DrawLine(_realtorSenses.transform.position, npc.transform.position);
        }
    }

    private bool IsRealtor()
    {
        return _realtorSenses != null;
    }

}
