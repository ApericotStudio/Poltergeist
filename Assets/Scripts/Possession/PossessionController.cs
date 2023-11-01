using Cinemachine;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
    private Highlight target;
    private IPossessable currentPossession;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        currentPossession = GetComponent<IPossessable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }
    }

    private void FixedUpdate()
    {
        LookForPossessables();
    }

    /// <summary>
    /// If camera is looking at possessable object possess it and unpossess current possession
    /// </summary>
    private void Possess()
    {
        IPossessable possessable = LookForPossessableObject();
        if (possessable == null)
        {
            return;
        }
        currentPossession.Unpossess();
        possessable.Possess();
        currentPossession = possessable;
    }

    /// <summary>
    /// Checks if the mainCamera is pointing at an object with the IPossessable interface
    /// </summary>
    /// <returns>The found IPossessable interface or null when no IPossessable interface is found</returns>
    private IPossessable LookForPossessableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent(out IPossessable possessableObject))
            {
                return possessableObject;
            }
        }
        return null;
    }


    /// <summary>
    /// Highlight objects with highlight script when looking at them. Dev note: This should be in a different class.
    /// </summary>
    private void LookForPossessables()
    {
        Highlight temp = null;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent(out Highlight highlight))
            {
                temp = highlight;
            }
        }
        if (temp == target)
        {
            return;
        }
        if (target != null)
        {
            target.ToggleHighlight(false);
        }
        target = temp;
        if (target != null)
        {
            target.ToggleHighlight(true);
        }
    }
}
