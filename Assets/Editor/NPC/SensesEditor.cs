using UnityEditor;
using UnityEngine;

/// <summary>
/// The editor for the NPC's senses. Draws the detection range, field of view, and hearing radius.
/// </summary>
[CustomEditor (typeof (BaseSenses), true)]
public class SensesEditor : Editor
{
    private BaseSenses _senses;
    private RealtorSenses _realtorSenses;

    private void OnEnable() {
        _senses = (BaseSenses)target;
        _realtorSenses = _senses.GetComponent<RealtorSenses>();
    }
	private void OnSceneGUI() {
		_senses = (BaseSenses)target;
        DrawHearingRadius();
        DrawFieldOfView();
        DrawObjectDetectionLine();
        if(IsRealtor())
        {
            DrawFearReductionRange();
            DrawVisitorDetectionLines();
        }
	}

    /// <summary>
    /// Draws the radius around the realtor that reduces visitor fear.
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
    /// Draws the npc's hearing radius on screen, which is the npc's auditory range.
    /// </summary>
    private void DrawHearingRadius()
    {
        Handles.color = new Color(1, 1, 0, 0.1f); // Yellow color with 50% transparency
        Handles.DrawSolidArc(_senses.transform.position, Vector3.up, Vector3.forward, 360, _senses.AuditoryRange);
    }

    /// <summary>
    /// Draws the npc's field of view on screen, which is the npc's sight range and field of view angle.
    /// </summary>
    private void DrawFieldOfView()
    {
        Vector3 viewAngle = DirFromAngle(-_senses.FieldOfViewAngle / 2, false);

        Handles.color = new Color(0, 0, 1, 0.2f); // Blue color with 50% transparency
        Handles.DrawSolidArc(_senses.transform.position, Vector3.up, viewAngle, _senses.FieldOfViewAngle, _senses.SightRange);
    }

    /// <summary>
    /// Draws a line from the NPC to the detected clutter. The line is red if the clutter is visible, and yellow if the clutter is audible.
    /// </summary>
    private void DrawObjectDetectionLine()
    {
        foreach (var (observableObject, detectedProperties) in _senses.DetectedObjects) {
            if(detectedProperties.IsVisible)
            {
                Handles.color = Color.blue;
                Handles.DrawLine(_senses.transform.position, observableObject.transform.position);
            }
            else if(detectedProperties.IsAudible)
            {
                Handles.color = Color.yellow;
                Handles.DrawLine(_senses.transform.position, observableObject.transform.position);
            }
        }
    }

    /// <summary>
    /// Draws a line from the realtor to the detected visitor. 
    /// </summary>
    private void DrawVisitorDetectionLines()
    {
        foreach (var visitor in _realtorSenses.SoothedVisitors)
        {
            Handles.color = Handles.color = new Color(0, 1, 0);
            Handles.DrawLine(_realtorSenses.transform.position, visitor.transform.position);
        }
    }
    
    /// <summary>
    /// Returns a vector3 direction from an angle. Used for the field of view.
    /// </summary>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += _senses.HeadTransform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

    private bool IsRealtor()
    {
        return _realtorSenses != null;
    }

}
