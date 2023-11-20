using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private VisionController _visionController;
    private Highlight _currentHighlight;

    private bool _lookingAtPossesion;
    private void Start()
    {
        _visionController = GetComponent<VisionController>();
        _visionController.LookingAtChanged.AddListener(HandleHighlighting);
    }

    /// <summary>
    /// Handles turning highlights on and off when the player looks at a different object
    /// </summary>
    private void HandleHighlighting()
    {
        if (_currentHighlight != null)
        {
            _currentHighlight.Highlighted(false);
        }
        GameObject objectInView = _visionController.LookingAt;
        if (objectInView == null)
        {
            return;
        }
        if (!objectInView.TryGetComponent(out Highlight highlight))
        {
            _currentHighlight = null;
            return;
        }

        _currentHighlight = highlight;

        CheckHighlight();

        if (_lookingAtPossesion)
        {
            return;
        }

        _currentHighlight.Highlighted(true);
    }

    public void CheckHighlight()
    {
        if (_currentHighlight != null && _currentHighlight.GetComponent<Throwable>())
        {
            if (_currentHighlight.GetComponent<Throwable>()._possessed)
            {
                _currentHighlight.Highlighted(false);
                _lookingAtPossesion = true;
                return;
            }
        }
        _lookingAtPossesion = false;
    }
}
