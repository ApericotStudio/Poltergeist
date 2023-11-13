using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [Tooltip("Max range for activating highlight")]
    [SerializeField, Range(0f, 20f)] private float highlightRange;

    private Camera mainCamera;
    private Highlight currentHighlight;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        LookForHighlightableObject();
    }

    /// <summary>
    /// Highlight objects with highlight script when looking at them
    /// </summary>
    private void LookForHighlightableObject()
    {
        Highlight foundHighlight = null;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, highlightRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out Highlight highlight))
            {
                foundHighlight = highlight;
            }
        }
        if (foundHighlight == currentHighlight)
        {
            return;
        }
        if (currentHighlight != null)
        {
            currentHighlight.Highlighted(false);
        }
        currentHighlight = foundHighlight;
        if (currentHighlight != null)
        {
            currentHighlight.Highlighted(true);
        }
    }
}
